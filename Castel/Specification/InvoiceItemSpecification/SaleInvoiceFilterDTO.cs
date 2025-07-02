namespace Castel.Specification.InvoiceItemSpecification
{
    public class SaleInvoiceFilterDTO
    {
        public long? divId { get; set; }
        public long? ItemId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
