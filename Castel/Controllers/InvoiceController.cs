using AutoMapper;
using Castel.Core.Unit;
using Castel.DTO;
using Castel.Extensions;
using Castel.Models;
using Castel.Specification.InvoiceItemSpecification;
using Castel.Specification.InvoiceSpecification;
using Microsoft.AspNetCore.Mvc;

namespace Castel.Controllers
{
    public class InvoiceController : BaseController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<InvoiceController> _logger;
        private readonly InvoiceMasterFilterBuilder invoiceMasterFilterBuilder;
        private readonly InvoiceItemFilterBuilder invoiceItemFilterBuilder;

        public InvoiceController(IUnitOfWork unitOfWork, IMapper mapper, 
            ILogger<InvoiceController> logger,InvoiceMasterFilterBuilder invoiceMasterFilterBuilder,
            InvoiceItemFilterBuilder invoiceItemFilterBuilder)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this._logger = logger;
            this.invoiceMasterFilterBuilder = invoiceMasterFilterBuilder;
            this.invoiceItemFilterBuilder = invoiceItemFilterBuilder;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> BuyIndex()
        {
            return View();
        }
        public async Task<IActionResult> SaleIndex()
        {
            return View();
        }

        //عرض العناصر المباعة
        [HttpGet]
        public async Task<IActionResult> GetSaledItem(SaleInvoiceFilterDTO itemDTO,long? id = null)
        {
            var specification = unitOfWork.SaleInvoiceRepository.InjectSpecification(invoiceItemFilterBuilder, itemDTO);
            var SaleInvoice = await unitOfWork.SaleInvoiceRepository.GetAll(
                filter: i => id == null || i.SaleInvoiceMasterId == id,
                includeProperties: "Item,Division,Vendor",
                extendQuery:specification);
            var divisions = await unitOfWork.DivisionRepository.GetAll();
            ViewBag.Divisions = divisions.Select(d => new { d.id, d.Name }).ToList();
            //ViewBag.Divisions = new SelectList(divisions, "Id", "Name");
            var SaledItemList = new List<InvoiceItemDTO>();
            foreach (var item in SaleInvoice)
            {
                var saleItem = new InvoiceItemDTO
                {
                    Id = item.id,
                    Description = item.Item.Description,
                    DivisionName = item.Division.Name,
                    Price = item.NetAmount,
                    QTY = item.Qty,
                    Total = item.NetAmount * item.Qty,
                    CreatedBy = (await unitOfWork.AccountRepository.Get(u => u.Id == item.CreatedById)).FullName,
                    CreatedOn = item.CreatedAt
                };
                SaledItemList.Add(saleItem);
            }
            return View(SaledItemList);
        }

        //عرض العناصر المشتراة
        [HttpGet]
        public async Task<IActionResult> GetBuyingItem(long? id)
        {
            var BuyInvoice = await unitOfWork.BuyInvoiceRepository.GetAll(
                filter: i => id == null || i.BuyInvoiceMasterId == id,
                includeProperties: "Item,Division,Vendor");
            var divisions = await unitOfWork.DivisionRepository.GetAll();
            ViewBag.Divisions = divisions.Select(d => new { d.id, d.Name }).ToList();
            var BuiedItemList = new List<InvoiceItemDTO>();
            foreach (var item in BuyInvoice)
            {
                var saleItem = new InvoiceItemDTO
                {
                    Id = item.id,
                    Description = item.Item.Description,
                    DivisionName = item.Division.Name,
                    Price = item.Price,
                    QTY = item.Qty,
                    Total = item.Price * item.Qty,
                    CreatedBy = (await unitOfWork.AccountRepository.Get(u => u.Id == item.CreatedById)).FullName,
                    CreatedOn = item.CreatedAt
                };
                BuiedItemList.Add(saleItem);
            }
            return View(BuiedItemList);
        }

        // عرض كل فواتير البيع
        [HttpGet]
        public async Task<IActionResult> GetSaleInvoice(InvoiceMasterFilterDTO filterDTO)
        {
            var specification = unitOfWork.SaleInvoiceMasterRepository.InjectSpecification(invoiceMasterFilterBuilder,filterDTO);
            var saleInvoice = await unitOfWork.SaleInvoiceMasterRepository.GetAll(
                includeProperties: $"{nameof(SaleInvoiceMaster.SaleInvoices)}",
                extendQuery: specification);
            var SaleInvoiceList = new List<InvoiceDTO>();

            foreach (var Invoice in saleInvoice)
            {
                var createdByUser = await unitOfWork.AccountRepository.Get(u => u.Id == Invoice.CreatedById);
                var saleItems = await unitOfWork.SaleInvoiceRepository.GetAll(s => s.SaleInvoiceMasterId == Invoice.id);

                var invoice = new InvoiceDTO
                {
                    Id = Invoice.id,
                    NetAmount = (double)(saleItems?.Sum(x => x.Total) ?? 0),
                    CreatedBy = createdByUser != null ? createdByUser.FullName : "غير معروف",
                    CreatedOn = Invoice.CreatedAt,
                };
                SaleInvoiceList.Add(invoice);
            }
            return View(SaleInvoiceList);
        }

        // عرض كل فواتير الشراء
        [HttpGet]
        public async Task<IActionResult> GetBuingInvoice()
        {
            var buyInvoice = await unitOfWork.BuyInvoiceMasterRepository.GetAll(includeProperties:
                $"{nameof(BuyInvoiceMaster.BuyInvoices)}");
            var BuyInvoiceList = new List<InvoiceDTO>();

            foreach (var Invoice in buyInvoice)
            {
                var createdByUser = await unitOfWork.AccountRepository.Get(u => u.Id == Invoice.CreatedById);
                var buyItems = await unitOfWork.BuyInvoiceRepository.GetAll(s => s.BuyInvoiceMasterId == Invoice.id);
                var invoice = new InvoiceDTO
                {
                    Id = Invoice.id,
                    NetAmount = (double)(buyItems?.Sum(x => x.Total) ?? 0),
                    CreatedBy = createdByUser != null ? createdByUser.FullName : "غير معروف",
                    CreatedOn = Invoice.CreatedAt,
                };
                BuyInvoiceList.Add(invoice);
            }
            return View(BuyInvoiceList);
        }


        //انشاء فاتورة بيع
        [HttpGet]
        public ActionResult CreateSaleInvoice()
        {
            return View();
        }

        //ادخال فاتورة بيع
        [HttpPost]
        public async Task<ActionResult> CreateSaleInvoice([FromBody] ICollection<PutSaleItemDTO> itemsDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest("خطأ في الادخال");

            // الحصول على سعر الصرف مرة واحدة فقط
            var Dollar = (await unitOfWork.ExchangeRateRepository.GetAll(
                orderBy: o => o.OrderByDescending(c => c.CreatedAt)))
                .FirstOrDefault()?.exchangeRate ?? 1;
            if (Dollar <= 0)
                return BadRequest("سعر الصرف غير صالح");


            var saleInvoiceMaster = new SaleInvoiceMaster
            {
                SaleInvoices = new List<SaleInvoice>()
            };

            foreach (var item in itemsDTO)
            {
                var dbItem = await unitOfWork.ItemRepository.Get(i => i.id == item.Id).ValidateNotFound();
                // معالجة نوع المادة
                var itemType = Enum.Parse<ItemType>(item.Type, ignoreCase: true);
                var typeMultiplier = (int)itemType;
                var rate = item.PayType == "USD" ? Dollar : 1;
                var totalQty = item.QTY * typeMultiplier;

                var invoiceItem = new SaleInvoice
                {
                    ItemId = item.Id,
                    DivisionId = dbItem.DivisionId,
                    VendorId = dbItem.VendorId,
                    CreatedById = CurrentUserId,
                    PaymentType = PaymentType.SYP,
                    Dollar = (long)Dollar,
                    NetAmount = (int)(item.NetAmount * rate),
                    Qty = totalQty,
                    Total = (int)(item.NetAmount * rate) * totalQty
                };

                dbItem.Qty -= totalQty;
                if (dbItem.NetAmount == 0)
                {
                    if (rate == Dollar)
                    {
                        dbItem.NetAmount = (int)(item.NetAmount * rate);
                        dbItem.NetAmountInDollar = item.NetAmount;
                    }
                    else
                    {
                        dbItem.NetAmount = item.NetAmount;
                        dbItem.NetAmountInDollar = item.NetAmount / Dollar;
                    }
                }

                await unitOfWork.ItemRepository.Update(dbItem);
                saleInvoiceMaster.SaleInvoices.Add(invoiceItem);
            }
            saleInvoiceMaster.CreatedById = CurrentUserId;
            await unitOfWork.SaleInvoiceMasterRepository.Insert(saleInvoiceMaster);
            await unitOfWork.SaleInvoiceRepository.InsertRange(saleInvoiceMaster.SaleInvoices);
            await unitOfWork.Save();

            var invoice = (await unitOfWork.SaleInvoiceMasterRepository.GetAll(orderBy:
                o => o.OrderByDescending(c => c.CreatedAt))).First();

            return Ok(new
            {
                invoiceNumber = invoice.id,
                date = invoice.CreatedAt,
            });
        }


        //انشاء فاتورة شراء
        [HttpGet]
        public ActionResult CreateBuyInvoice()
        {
            return View();
        }
        //ادخال فاتورة شراء
        [HttpPost]
        public async Task<ActionResult> CreateBuyInvoice([FromBody] List<PutBuyItemDTO> itemsDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest("خطأ في الادخال");

            // الحصول على سعر الصرف مرة واحدة فقط
            var dollarRate = (await unitOfWork.ExchangeRateRepository.GetAll(
                orderBy: o => o.OrderByDescending(c => c.CreatedAt)))
                .FirstOrDefault()?.exchangeRate ?? 1;
            if (dollarRate <= 0)
                return BadRequest("سعر الصرف غير صالح");

            var buyInvoiceMaster = new BuyInvoiceMaster
            {
                CreatedById = CurrentUserId,
                BuyInvoices = new List<BuyInvoice>()
            };

            foreach (var item in itemsDTO)
            {
                var dbItem = await unitOfWork.ItemRepository
                    .Get(i => i.id == item.Id)
                    .ValidateNotFound();

                // معالجة نوع المادة
                var itemType = Enum.Parse<ItemType>(item.Type, ignoreCase: true);
                var typeMultiplier = (int)itemType;
                var rate = item.PayType == "USD" ? dollarRate : 1;
                var totalQty = item.QTY * typeMultiplier;

                // تحديث السعر
                await CheckPriceChange(dbItem, item);

                // إنشاء فاتورة الشراء
                var invoiceItem = new BuyInvoice
                {
                    ItemId = item.Id,
                    DivisionId = dbItem.DivisionId,
                    VendorId = dbItem.VendorId,
                    CreatedById = CurrentUserId,
                    Price = (int)(item.Price * rate),
                    Qty = totalQty,
                    Total = (int)(item.Price * rate) * totalQty
                };

                // تحديث كمية المادة
                dbItem.Qty += totalQty;
                var price = item.Price / typeMultiplier;
                // تحديث أسعار المادة
                if (rate == 1)
                {
                    dbItem.PriceInDollar = price / dollarRate;
                    dbItem.Price = price;
                }
                else
                {
                    dbItem.PriceInDollar = price;
                    dbItem.Price = price * dollarRate;
                }

                await unitOfWork.ItemRepository.Update(dbItem);
                buyInvoiceMaster.BuyInvoices.Add(invoiceItem);
            }

            await unitOfWork.BuyInvoiceRepository.InsertRange(buyInvoiceMaster.BuyInvoices);
            await unitOfWork.BuyInvoiceMasterRepository.Insert(buyInvoiceMaster);
            await unitOfWork.Save();

            // الحصول على رقم الفاتورة الجديدة
            var newInvoiceId = (await unitOfWork.BuyInvoiceMasterRepository.GetAll(
                orderBy: o => o.OrderByDescending(c => c.CreatedAt)))
                .First().id;

            return Ok(new { invoiceNumber = newInvoiceId });
        }

        public async Task CheckPriceChange(Item item, PutBuyItemDTO itemDTO)
        {
            try
            {
                if (item == null || itemDTO == null)
                    return;

                // الحصول على سعر الصرف مع معالجة حالة عدم التوفر
                var dollarRate = (await unitOfWork.ExchangeRateRepository.GetAll(
                    orderBy: o => o.OrderByDescending(c => c.CreatedAt)))
                    .FirstOrDefault()?.exchangeRate ?? 1;

                if (dollarRate <= 0)
                {
                    _logger.LogWarning("سعر الصرف غير صالح أو يساوي الصفر");
                    return;
                }

                // معالجة نوع المادة مع التحقق من الصحة
                if (!Enum.TryParse<ItemType>(itemDTO.Type, ignoreCase: true, out var itemType))
                {
                    _logger.LogWarning($"نوع المادة غير صالح: {itemDTO.Type}");
                    return;
                }

                var rate = itemDTO.PayType == "USD" ? dollarRate : 1;
                var multiplier = itemType switch
                {
                    ItemType.Piece => 1,
                    ItemType.Cruise => 10,
                    ItemType.Package => 20,
                    _ => 1
                };

                if ((item.Price * multiplier) != itemDTO.Price)
                {
                    var newPrice = new Pricing
                    {
                        CreatedById = CurrentUserId,
                        ItemId = item.id,
                        Old_Price = item.PriceInDollar,
                        New_Price = rate == 1 ? itemDTO.Price * dollarRate * multiplier : itemDTO.Price * multiplier,
                        Old_NetAmount = item.NetAmount,
                        New_NetAmount = item.NetAmount,
                    };

                    await unitOfWork.PriceRepository.Insert(newPrice);
                    // تم نقل unitOfWork.Save() للدالة الرئيسية لتحسين الأداء
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في تحديث السعر للمادة {ItemId}", item?.id);
                throw; // أو يمكنك التعامل مع الخطأ حسب متطلباتك
            }
        }
    }
}
