namespace InvoiceService.Interfaces
{
    public interface IAddress
    {
        string StreetAddress { get; }

        string City { get; }

        ICountry Country { get; }
    }
}
