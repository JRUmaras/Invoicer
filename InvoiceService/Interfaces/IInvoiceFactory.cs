namespace InvoiceService.Interfaces
{
    public interface IInvoiceFactory
    {
        IInvoice CreateInvoice(IOrder order, decimal vatRate);
    }
}
