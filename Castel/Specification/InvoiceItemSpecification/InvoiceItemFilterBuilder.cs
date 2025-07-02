using Castel.Models;

namespace Castel.Specification.InvoiceItemSpecification
{
    public class InvoiceItemFilterBuilder : GenericFiltersBuilder<SaleInvoice, SaleInvoiceFilterDTO>
    {
        public override void InitItemSpecifications()
        {
            AddSpecification(
                nameof(SaleInvoiceFilterDTO.ItemId),
                filter => new InvoiceItemSpecification(filter.ItemId));

            AddSpecification(
                nameof(SaleInvoiceFilterDTO.divId),
                filter => new InvoiceDivisionIdSpecification(filter.divId));

            AddSpecification(
                nameof(SaleInvoiceFilterDTO.StartDate),
                filter => new InvoiceDateSpecification(filter.StartDate,filter.EndDate));
        }
    }
}
