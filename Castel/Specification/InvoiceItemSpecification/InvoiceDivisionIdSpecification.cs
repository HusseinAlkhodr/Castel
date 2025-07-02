using Castel.Models;

namespace Castel.Specification.InvoiceItemSpecification
{
    public class InvoiceDivisionIdSpecification : BaseSpecification<SaleInvoice>
    {
        public InvoiceDivisionIdSpecification(long? divId)
        {
            if (divId != null)
                SetCriteria(i => i.DivisionId == divId);
        }
    }
}
