using System;
using System.Collections.Generic;
using System.Linq;
using InvoiceService.Interfaces;
using InvoiceService.Services;
using NSubstitute;
using NUnit.Framework;

namespace InvoicerTests
{
    public class DefaultInvoiceFactoryTests
    {
        private readonly DefaultInvoiceFactory _invoiceFactory = new ();

        private IOrder _order;

        [SetUp]
        public void Setup()
        {
            _order = Substitute.For<IOrder>();
        }

        [TestCase(0, 0, TestName = "Empty Order & Zero VAT Rate")]
        [TestCase(1, 0, TestName = "Single-Item Order & Zero VAT Rate")]
        [TestCase(2, 0, TestName = "Multiple-Items Order & Zero VAT Rate")]
        [TestCase(0, 0.1, TestName = "Empty Order & Non-zero VAT Rate")]
        [TestCase(1, 0.1, TestName = "Single-Item Order & Non-zero VAT Rate")]
        [TestCase(2, 0.1, TestName = "Multiple-Items Order & Non-zero VAT Rate")]
        public void NormalInvoiceGeneration(int numberOfItemsInOrder, decimal vatRate)
        {
            // Arrange

            var items = GenerateOrderItemsList(numberOfItemsInOrder);

            _order.Buyer.Returns(Substitute.For<IPerson>());
            _order.Seller.Returns(Substitute.For<IPerson>());
            _order.Items.Returns(items);

            var expectedSubTotal = items.Sum(item => item.Price);
            var expectedTotal = expectedSubTotal * (1 + vatRate);
            var expectedVat = expectedSubTotal * vatRate;

            // Act

            var invoice = _invoiceFactory.CreateInvoice(_order, vatRate);

            // Assert

            Assert.NotNull(invoice);
            Assert.NotNull(invoice.OrderItems);
            Assert.NotNull(invoice.Seller);
            Assert.NotNull(invoice.Buyer);

            Assert.AreEqual(vatRate, invoice.VatRate);
            Assert.AreEqual(expectedSubTotal, invoice.SubTotal);
            Assert.AreEqual(expectedTotal, invoice.Total);
            Assert.AreEqual(expectedVat, invoice.Vat);
        }

        [TestCase(TestName = "Order is null")]
        public void InvoiceGenerationWhenOrderIsNull()
        {
            // Act & Assert

            Assert.Throws<ArgumentNullException>(() => _invoiceFactory.CreateInvoice(null, 0));
        }

        private static List<IItem> GenerateOrderItemsList(int numberOfItemsInOrder)
        {
            var items = new List<IItem>(numberOfItemsInOrder);

            for (var idx = 1; idx <= numberOfItemsInOrder; ++idx)
            {
                var item = Substitute.For<IItem>();
                item.Price.Returns(100m * idx);
                items.Add(item);
            }

            return items;
        }
    }
}
