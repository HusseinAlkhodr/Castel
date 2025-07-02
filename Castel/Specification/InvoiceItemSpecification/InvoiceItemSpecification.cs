using Castel.DTO;
using Castel.Models;

namespace Castel.Specification.InvoiceItemSpecification
{
    public class InvoiceItemSpecification : BaseSpecification<SaleInvoice>
    {
        public InvoiceItemSpecification( long? ItemId)
        {
            if (ItemId != null)
                SetCriteria(i  => i.ItemId == ItemId);
        }
    }
}
