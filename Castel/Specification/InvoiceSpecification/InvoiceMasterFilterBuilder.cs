using Castel.Models;
using Castel.Specification.InvoiceItemSpecification;

namespace Castel.Specification.InvoiceSpecification
{
    public class InvoiceMasterFilterBuilder : GenericFiltersBuilder<SaleInvoiceMaster, InvoiceMasterFilterDTO>
    {
        public override void InitItemSpecifications()
        {
            AddSpecification(
                nameof(InvoiceMasterFilterDTO.Name),
                filter => new InvoiceMasterNameSpecification(filter.Name));

            AddSpecification(
                nameof(InvoiceMasterFilterDTO.StartDate),
                filter => new InvoiceMasterDateSpecification(filter.StartDate,filter.EndDate));
        }
    }
}
