namespace InvoiceService.Interfaces
{
    public interface IVatFinder
    {
        decimal GetVatRate(IPerson seller, IPerson buyer);
    }
}