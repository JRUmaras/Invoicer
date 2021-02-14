namespace InvoiceService.Interfaces
{
    public interface ICountry
    {
        string Name { get; }

        bool IsInEuropeanUnion { get; }

        decimal VatRate { get; }
    }
}