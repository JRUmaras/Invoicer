using System;
using InvoiceService.Interfaces;
using InvoiceService.Services;
using NSubstitute;
using NUnit.Framework;

namespace InvoicerTests
{
    public class InvoiceGeneratorTests
    {
        private InvoiceGenerator _invoiceGenerator;

        private IVatFinder _vatFinder;
        private IInvoiceFactory _invoiceFactory;

        private IOrder _order;

        [SetUp]
        public void Setup()
        {
            _vatFinder = Substitute.For<IVatFinder>();
            _invoiceFactory = Substitute.For<IInvoiceFactory>();

            _order = Substitute.For<IOrder>();
        }

        [TestCase(TestName = "Normal invoice generation")]
        public void NormalInvoiceGeneration()
        {
            // Arrange

            _invoiceGenerator = new InvoiceGenerator(_vatFinder, _invoiceFactory);

            // Act

            var invoice = _invoiceGenerator.GenerateInvoice(_order);

            // Assert

            Assert.NotNull(invoice);
        }

        [TestCase(TestName = "No VAT finder")]
        public void InvoiceGenerationWithNullVatFinder()
        {
            // Act & Assert

            Assert.Throws<ArgumentNullException>(() => _invoiceGenerator = new InvoiceGenerator(null, _invoiceFactory));
        }

        [TestCase(TestName = "Undefined invoice factory")]
        public void InvoiceGenerationWithNullInvoiceFactory()
        {
            // Act & Assert

            Assert.Throws<ArgumentNullException>(() => _invoiceGenerator = new InvoiceGenerator(_vatFinder, null));
        }

        [TestCase(TestName = "Undefined order")]
        public void InvoiceGenerationWithNullOrder()
        {
            // Arrange

            _invoiceGenerator = new InvoiceGenerator(_vatFinder, _invoiceFactory);

            // Act & Assert

            Assert.Throws<ArgumentNullException>(() => _invoiceGenerator.GenerateInvoice(null));
        }
    }
}
