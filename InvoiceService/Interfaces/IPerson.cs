namespace InvoiceService.Interfaces
{
    public interface IPerson
    {
        string Name { get; }

        bool IsVatPayer { get; }

        IAddress Address { get; }
    }
}