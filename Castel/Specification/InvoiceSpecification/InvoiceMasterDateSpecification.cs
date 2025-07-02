using Castel.Models;

namespace Castel.Specification.InvoiceSpecification
{
    public class InvoiceMasterDateSpecification : BaseSpecification<SaleInvoiceMaster>
    {
        public InvoiceMasterDateSpecification(DateTime? StartDate, DateTime? EndDate)
        {
            if (StartDate != null)
                SetCriteria(m => m.CreatedAt >= StartDate && (EndDate == null ? true : m.CreatedAt <= EndDate));
        }
    }
}
