using Castel.DTO;
using Castel.Models;

namespace Castel.Specification.InvoiceItemSpecification
{
    public class InvoiceDateSpecification : BaseSpecification<SaleInvoice>
    {
        public InvoiceDateSpecification(DateTime? startDate, DateTime? endDate)
        {
            if (startDate != null)
                SetCriteria(i => i.CreatedAt >= startDate && (endDate == null ? true : i.CreatedAt <= endDate));
        }
    }
}
