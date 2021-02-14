using System.Collections.Generic;

namespace InvoiceService.Interfaces
{
    public interface IOrder
    {
        IEnumerable<IItem> Items { get; }

        IPerson Seller { get; }

        IPerson Buyer { get; }
    }
}