using AutoMapper;
using Castel.Core.Unit;
using Castel.DTO;
using Castel.Extensions;
using Castel.Middlewares;
using Castel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Reflection;


namespace Castel.Controllers
{
    public class ItemController : BaseController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public ItemController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        // GET: ItemController
        public async Task<IActionResult> Index()
        {
            var items = await unitOfWork.ItemRepository.GetAll(includeProperties: "Division,Vendor");
            var itemList = mapper.Map<IEnumerable<GetItemDTO>>(items);
            foreach (var item in itemList)
                item.typeName = item.Type.ToString();
            return View(itemList);
        }

        // GET: ItemController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var item = await unitOfWork.ItemRepository.Get(i => i.id == id, includeProperties: "Division,Vendor").ValidateNotFound();
            var itemDTO = mapper.Map<GetItemDTO>(item);
            itemDTO.typeName = itemDTO.Type.ToString();
            return View(itemDTO);
        }
        // GET: ItemController/Create
        public async Task<ActionResult> Create()
        {

            var Divisions = await unitOfWork.DivisionRepository.GetAll();
            var Vendors = await unitOfWork.VendorRepository.GetAll();
            ViewBag.Divisions = Divisions.Select(d => new SelectListItem
            {
                Value = d.id.ToString(),
                Text = d.Name
            }).ToList();
            ViewBag.Vendors = Vendors.Select(d => new SelectListItem
            {
                Value = d.id.ToString(),
                Text = d.Name
            }).ToList();
            return View(nameof(Create));
        }

        // POST: ItemController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddItemDTO itemDTO)
        {
            if (!ModelState.IsValid)
                throw new HusseinErrorResponseException("");
            var Rate = (await unitOfWork.ExchangeRateRepository.GetAll(
                orderBy: o => o.OrderByDescending(
                    e => e.CreatedAt))
                ).First().exchangeRate;

            var item = new Item
            {
                Barcode = itemDTO.Barcode,
                Description = itemDTO.Description,
                Type = ItemType.Piece,
                DivisionId = itemDTO.DivisionId,
                VendorId = itemDTO.VendorId,
                CreatedById = CurrentUserId,
            };

            await unitOfWork.ItemRepository.Insert(item);
            await unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        // GET: ItemController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var item = await unitOfWork.ItemRepository.Get(i => i.id == id).ValidateNotFound();
            var itemDTO = mapper.Map<UpdateItemDTO>(item);
            return View(itemDTO);
        }

        // POST: ItemController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UpdateItemDTO itemDTO)
        {
            if (!ModelState.IsValid)
                throw new HusseinErrorResponseException("");
            var item = await unitOfWork.ItemRepository.Get(i => i.id == itemDTO.Id);
            item.Barcode = itemDTO.Barcode;
            item.Description = itemDTO.Description;
            item.NetAmount = itemDTO.NetAmount;
            item.NetAmountInDollar = itemDTO.NetAmountDollar;
            item.UpdatedAt = DateTime.UtcNow.ToLocalTime();
            item.UpdatedById = CurrentUserId;
            await unitOfWork.ItemRepository.Update(item);
            await unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        // GET: ItemController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var item = await unitOfWork.ItemRepository.Get(i => i.id == id).ValidateNotFound();
            var itemDTO = mapper.Map<DeleteItemDTO>(item);
            return View(itemDTO);
        }

        // POST: ItemController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteItemDTO itemDTO)
        {
            await unitOfWork.ItemRepository.Get(u => u.id == itemDTO.Id).ValidateNotFound();
            var item = mapper.Map<Item>(itemDTO);
            await unitOfWork.ItemRepository.Delete(item);
            await unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        // GET: ItemController/EditPrice/5
        [HttpGet]
        public async Task<IActionResult> EditPrice(int id)
        {
            var item = await unitOfWork.ItemRepository.Get(i => i.id == id).ValidateNotFound();
            var itemDTO = mapper.Map<EditPrice>(item);
            return View(itemDTO);
        }
        // Post: ItemController/EditPrice/5
        [HttpPost]
        public async Task<IActionResult> EditPrice(EditPrice itemDTO)
        {
            var item = await unitOfWork.ItemRepository.Get(i => i.id == itemDTO.Id).ValidateNotFound();

            Pricing pricing = new Pricing
            {
                ItemId = itemDTO.Id,
                Old_Price = item.PriceInDollar,
                New_Price = itemDTO.PriceInDollar,
                Old_NetAmount = item.NetAmountInDollar,
                New_NetAmount = itemDTO.NetAmountInDollar,
                CreatedById = CurrentUserId
            };
            item.PriceInDollar = itemDTO.PriceInDollar;
            item.Price = itemDTO.PriceInDollar * (await GetDollarRate());
            item.NetAmountInDollar = itemDTO.NetAmountInDollar;
            item.NetAmount = itemDTO.NetAmountInDollar * (await GetDollarRate());
            item.UpdatedAt = DateTime.UtcNow;
            item.UpdatedById = CurrentUserId;
            await unitOfWork.ItemRepository.Update(item);
            await unitOfWork.PriceRepository.Insert(pricing);
            await unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        // للشراء
        public async Task<IActionResult> GetForBuy(string barcode)
        {
            var item = await unitOfWork.ItemRepository.Get(i => i.Barcode == barcode).ValidateNotFound();
            var itemDTO = new GetBuyItemDTO
            {
                Id = item.id,
                Barcode = item.Barcode,
                Description = item.Description,
                Price = item.Price,
                PriceInDollar = item.PriceInDollar,
                Dollar = await GetDollarRate()
            };
            return Ok(itemDTO);
        }
        // للبيع
        public async Task<IActionResult> GetForSale(string barcode)
        {
            var item = await unitOfWork.ItemRepository.Get(i => i.Barcode == barcode).ValidateNotFound();
            var itemDTO = new GetSaleItemDTO
            {
                Id = item.id,
                Barcode = item.Barcode,
                Description = item.Description,
                NetAmount = item.NetAmount,
                NetAmountInDollar = item.NetAmountInDollar,
                Dollar = await GetDollarRate()
            };
            return Ok(itemDTO);
        }
        // For immediate price change
        public async Task<IActionResult> ChangePrice(double rate, string returnUrl)
        {
            var dollar = new ExchangeRate { exchangeRate = rate, CreatedAt = DateTime.UtcNow.ToLocalTime() };
            await unitOfWork.ExchangeRateRepository.Insert(dollar);
            await unitOfWork.Save();
            var items = await unitOfWork.ItemRepository.GetAll();
            foreach (var item in items)
            {
                item.Price = item.PriceInDollar * rate;
                item.NetAmount = item.NetAmountInDollar * rate;
                await unitOfWork.ItemRepository.Update(item);
                await unitOfWork.Save();
            }
            if (!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index", "Invoice");
        }
        public async Task<double> GetDollarRate()
        {
            return (await unitOfWork.ExchangeRateRepository.GetAll(orderBy: o => o.OrderByDescending(c => c.CreatedAt))).First().exchangeRate;
        }

        [HttpGet]
        public async Task<IActionResult> SearchForPurchase(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return Ok(new List<ItemDTO>());
            var lowerTerm = term.ToLower();
            var results = await unitOfWork.ItemRepository.GetAll(
                x =>
                x.Description != null && 
                x.Barcode != null &&
                (x.Description.ToLower().Contains(lowerTerm) || x.Barcode.Contains(lowerTerm))
                );
            var itemdto = mapper.Map<List<ItemDTO>>(results);
            return Ok(itemdto);
        }

    }
}
