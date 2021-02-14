using System;
using InvoiceService.Interfaces;

namespace InvoiceService.Services
{
    public class InvoiceGenerator
    {
        private readonly IVatFinder _vatFinder;
        private readonly IInvoiceFactory _invoiceFactory;

        public InvoiceGenerator(IVatFinder vatFinder, IInvoiceFactory invoiceFactory)
        {
            _vatFinder = vatFinder ?? throw new ArgumentNullException(nameof(vatFinder));
            _invoiceFactory = invoiceFactory ?? throw new ArgumentNullException(nameof(invoiceFactory));
        }

        public IInvoice GenerateInvoice(IOrder order)
        {
            if (order is null) throw new ArgumentNullException(nameof(order));

            var vatRate = _vatFinder.GetVatRate(order.Seller, order.Buyer);

            var invoice = _invoiceFactory.CreateInvoice(order, vatRate);

            return invoice;
        }
    }
}
