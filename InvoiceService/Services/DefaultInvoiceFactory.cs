using InvoiceService.Interfaces;
using InvoiceService.Models;

namespace InvoiceService.Services
{
    public class DefaultInvoiceFactory : IInvoiceFactory
    {
        public IInvoice CreateInvoice(IOrder order, decimal vatRate) => new DefaultInvoice(order, vatRate);
    }
}
