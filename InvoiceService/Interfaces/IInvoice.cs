using System;
using System.Collections.Generic;

namespace InvoiceService.Interfaces
{
    public interface IInvoice
    {
        IEnumerable<IItem> OrderItems { get; }

        IPerson Seller { get; }

        IPerson Buyer { get; }

        DateTime Date { get; }

        decimal VatRate { get; }

        decimal SubTotal { get; }

        decimal Total { get; }

        decimal Vat { get; }
    }
}