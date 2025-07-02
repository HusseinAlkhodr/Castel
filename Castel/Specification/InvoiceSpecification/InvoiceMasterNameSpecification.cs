using Castel.Models;

namespace Castel.Specification.InvoiceSpecification
{
    public class InvoiceMasterNameSpecification : BaseSpecification<SaleInvoiceMaster>
    {
        public InvoiceMasterNameSpecification(string? Name)
        {
            if (Name != null)
                SetCriteria(m => m.ClientName == Name);
        }
    }
}
