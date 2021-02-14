using InvoiceService.Interfaces;
using InvoiceService.Services;
using NSubstitute;
using NUnit.Framework;

namespace InvoicerTests
{
    public class DefaultVatFinderTests
    {
        private DefaultVatFinder _vatFinder;

        private readonly IPerson _seller = Substitute.For<IPerson>();
        private readonly IPerson _buyer = Substitute.For<IPerson>();

        private readonly ICountry _sellerCountry = Substitute.For<ICountry>();
        private readonly ICountry _buyerCountry = Substitute.For<ICountry>();

        private const string _sellerCountryName = "Germany";
        private const decimal _sellerCountryVatRate = 0.1m;

        [SetUp]
        public void Setup()
        {
            _vatFinder = new DefaultVatFinder();

            _sellerCountry.Name.Returns(_sellerCountryName);
            _sellerCountry.VatRate.Returns(_sellerCountryVatRate);

            _seller.Address.Country.Returns(_sellerCountry);

            _buyer.Address.Country.Returns(_buyerCountry);
        }

        [TestCase(TestName = "Seller not VAT Payer & Expected VAT = 0")]
        public void SellerIsNotVatPayer()
        {
            // Make sure that
            // - buyer being in/outside EU
            // - buyer being a VAT (non)payer
            // - buyer being in the same/different country as seller
            // has no effect on the result, hence the looping
            foreach (var isBuyerInEu in new[] { true, false })
            {
                foreach (var isBuyerAndSellerInSameCountry in new[] { true, false })
                {
                    foreach (var isBuyerVatPayer in new[] { true, false })
                    {
                        // Arrange

                        SubstituteSetup(isSellerVatPayer: false, isBuyerInEu, isBuyerAndSellerInSameCountry, isBuyerVatPayer);

                        const int expectedVatRate = 0;

                        // Act

                        var vatRate = _vatFinder.GetVatRate(_seller, _buyer);

                        // Assert

                        Assert.AreEqual(expectedVatRate, vatRate);
                    }
                }
            }
        }

        [TestCase(TestName = "Seller VAT Payer & Buyer not in EU & Expected VAT = 0")]
        public void SellerIsVatPayerBuyerOutsideEu()
        {
            // Make sure that
            // - buyer being a VAT (non)payer
            // - buyer being in the same/different country as seller
            // has no effect on the result, hence the looping
            foreach (var isBuyerAndSellerInSameCountry in new[] { true, false })
            {
                foreach (var isBuyerVatPayer in new[] { true, false })
                {
                    // Arrange

                    SubstituteSetup(isSellerVatPayer: true, isBuyerInEu: false, isBuyerAndSellerInSameCountry, isBuyerVatPayer);

                    const int expectedVatRate = 0;

                    // Act

                    var vatRate = _vatFinder.GetVatRate(_seller, _buyer);

                    // Assert

                    Assert.AreEqual(expectedVatRate, vatRate);
                }
            }
        }

        [TestCase(TestName = "Seller VAT Payer & Buyer in EU & Same Country & Expected VAT > 0")]
        public void SellerIsVatPayerBuyerInEuAndSameCountry()
        {
            // Make sure that
            // - buyer being in the same/different country as seller
            // has no effect on the result, hence the looping
            foreach (var isBuyerVatPayer in new[] { true, false })
            {
                // Arrange

                SubstituteSetup(isSellerVatPayer: true, isBuyerInEu: true, isBuyerAndSellerInSameCountry: true, isBuyerVatPayer);

                var expectedVatRate = _buyerCountry.VatRate;

                // Act

                var vatRate = _vatFinder.GetVatRate(_seller, _buyer);

                // Assert

                Assert.AreEqual(expectedVatRate, vatRate);
            }
        }

        [TestCase(false, TestName = "Seller VAT Payer & Buyer in EU & Not Same Country & Buyer not VAT Payer & Expected VAT > 0")]
        [TestCase(true, TestName = "Seller VAT Payer & Buyer in EU & Not Same Country & Buyer VAT Payer & Expected VAT = 0")]
        public void SellerIsVatPayerBuyerInEuNotSameCountry(bool isBuyerVatPayer)
        {
            // Arrange

            SubstituteSetup(isSellerVatPayer: true, isBuyerInEu: true, isBuyerAndSellerInSameCountry: false, isBuyerVatPayer);

            var expectedVatRate = isBuyerVatPayer ? 0 : _buyerCountry.VatRate;

            // Act

            var vatRate = _vatFinder.GetVatRate(_seller, _buyer);

            // Assert

            Assert.AreEqual(expectedVatRate, vatRate);
        }

        public void SubstituteSetup(bool isSellerVatPayer, bool isBuyerInEu, bool isBuyerAndSellerInSameCountry, bool isBuyerVatPayer)
        {
            _seller.IsVatPayer.Returns(isSellerVatPayer);

            if (isBuyerAndSellerInSameCountry)
            {
                _buyerCountry.Name.Returns(_sellerCountryName);
                _buyerCountry.VatRate.Returns(_sellerCountryVatRate);
            }
            else
            {
                _buyerCountry.Name.Returns(_sellerCountryName + 1);
                _buyerCountry.VatRate.Returns(_sellerCountryVatRate + 0.1m);
            }
            _buyerCountry.IsInEuropeanUnion.Returns(isBuyerInEu);

            _buyer.IsVatPayer.Returns(isBuyerVatPayer);
        }
    }
}