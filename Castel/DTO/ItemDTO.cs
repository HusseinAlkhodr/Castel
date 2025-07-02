using System.ComponentModel.DataAnnotations;
using Castel.Models;

namespace Castel.DTO
{
    public class ItemDTO
    {

        public string? Barcode { get; set; }
        public string? Description { get; set; }
        public int Qty { get; set; }
        public ItemType Type { get; set; }
        public double Price { get; set; }
        public double NetAmount { get; set; }
        public double PriceInDollar { get; set; }
        public double NetAmountInDollar { get; set; }
        public long VendorId { get; set; }
        public long DivisionId { get; set; }
    }
    public class Invoice : ItemDTO
    {
        public long Id { get; set; }
        public string TypeName { get; set; }
    }
    public class AddItemDTO
    {
        public string? Barcode { get; set; }
        public string? Description { get; set; }
        public long VendorId { get; set; }
        public long DivisionId { get; set; }
    }
    public class DeleteItemDTO
    {
        public long Id { get; set; }
        public string? Barcode { get; set; }
        public string? Description { get; set; }
    }
    public class UpdateItemDTO 
    {
        public long Id { get; set; }
        public string? Barcode { get; set; }
        public string? Description { get; set; }
        public double NetAmount { get; set; }
        public double NetAmountDollar { get; set; }
    }
    public class GetItemDTO : ItemDTO
    {
        public long id { get; set; }
        public double Price { get; set; }
        public double NetAmount { get; set; }
        public string? typeName { get; set; }
        public string? DivisionName { get; set; }
        public string? VendorName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
    public class EditPrice
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public double PriceInDollar { get; set; }
        public double NetAmountInDollar { get; set; }

    }
    
    public class GetInvoice : Invoice
    {
        public DateTime CreatedAt { get; set; }
    }
    public class ExchangeDTO
    {
        public double rate { get; set; }
    }
    public class PriceDTO
    {
        public string Description { get; set; }
        public double OldPrice { get; set; }
        public double NewPrice { get; set; }
        public double OldNetAmount { get; set; }
        public double NewNetAmount { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
