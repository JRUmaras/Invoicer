using InvoiceService.Interfaces;

namespace InvoiceService.Services
{
    public class DefaultVatFinder : IVatFinder
    {
        /*
                Is seller VAT payer?
                /                  \
            no /                    \ yes
              /                      \
          VAT = 0                Is buyer in EU?
                                /               \
                            no /                 \ yes
                              /                   \
                           VAT = 0      Are seller and buyer in the same country?
                                             /                   \
                                         no /                     \ yes
                                           /                       \
                                    Is buyer VAT payer?          VAT > 0
                                    /                 \
                                no /                   \ yes
                                  /                     \
                             VAT > 0                  VAT = 0
        */
        public decimal GetVatRate(IPerson seller, IPerson buyer)
        {
            if (seller.IsVatPayer
                && buyer.Address.Country.IsInEuropeanUnion
                && (seller.Address.Country.Name == buyer.Address.Country.Name || !buyer.IsVatPayer))
            {
                return buyer.Address.Country.VatRate;
            }

            return 0;
        }
    }
}
