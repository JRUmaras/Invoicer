namespace InvoiceService.Interfaces
{
    public interface IItem
    {
        string Name { get; }

        int Quantity { get; }

        decimal UnitPrice { get; }

        decimal Price { get; }
    }
}