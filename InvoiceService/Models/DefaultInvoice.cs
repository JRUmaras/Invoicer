using System;
using System.Collections.Generic;
using System.Linq;
using InvoiceService.Interfaces;

namespace InvoiceService.Models
{
    internal class DefaultInvoice : IInvoice
    {
        private readonly IOrder _order;

        public IEnumerable<IItem> OrderItems => _order.Items;

        public IPerson Seller => _order.Seller;

        public IPerson Buyer => _order.Buyer;

        public DateTime Date { get; }

        public decimal VatRate { get; }

        public decimal SubTotal => OrderItems.Sum(item => item.Price);

        public decimal Total => SubTotal * (1 + VatRate);

        public decimal Vat => SubTotal * VatRate;

        public DefaultInvoice(IOrder order, decimal vatRate)
        {
            _order = order ?? throw new ArgumentNullException(nameof(order));

            VatRate = vatRate;

            Date = DateTime.Now;
        }
    }
}
