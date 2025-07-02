using Castel.Models;

namespace Castel.DTO
{
    public class InvoiceDTO
    {
        public long Id { get; set; }
        public double NetAmount { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }

    }
    public class InvoiceItemDTO
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public string DivisionName { get; set; }
        public double Price { get; set; }
        public int QTY { get; set; }
        public double Total { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
    public class AddInvoiceItemsDTO
    {
        public long id { get; set; }
        public string barcode { get; set; }
        public string description { get; set; }
        public string Type { get; set; }
        public string typeName{ get; set; }
        public double NetAmount { get; set; }
        public int QTY { get; set; }
    }
    public class AddInvoiceItemDTO : ItemDTO
    {
        public long Id { get; set; }
        public string TypeName { get; set; }
        public double Dollar { get; set; }
    }


    public class GetBuyItemDTO
    {
        public long Id { get; set; }
        public string Barcode { get; set; }
        public string Description { get; set; }
        public double PriceInDollar { get; set; }
        public double Price { get; set; }
        public double Dollar { get; set; }
    }
    public class GetSaleItemDTO
    {
        public long Id { get; set; }
        public string Barcode { get; set; }
        public string Description { get; set; }
        public double NetAmount { get; set; }
        public double NetAmountInDollar { get; set; }
        public double Dollar { get; set; }
    }
    //العناصر القادمة من فاتورة الشراء
    public class PutBuyItemDTO
    {
        public long Id { get; set; }
        public string Barcode { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public double Price { get; set; }
        public int QTY { get; set; }
        public string PayType { get; set; }
    }
    public class PutSaleItemDTO
    {
        public long Id { get; set; }
        public string Barcode { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public double NetAmount { get; set; }
        public int QTY { get; set; }
        public string PayType { get; set; }
    }
}
