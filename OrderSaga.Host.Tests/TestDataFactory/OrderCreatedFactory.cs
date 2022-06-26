using MoreLinq;
using OrderSaga.Contracts;
using OrderSaga.Contracts.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderSaga.Host.Tests.TestDataFactory
{
    public static class OrderCreatedFactory
    {
        public static OrderCreated OrderCreated(
            this ITestDataFactory factory,
            Guid? orderId = null,
            int? orderNumber = null,
            DateTime? orderDate = null,
            string customerName = null,
            string customerSurname = null,
            ICollection<OrderItemDto> items = null)
        {
            return new OrderCreated(
                orderId: orderId ?? Guid.NewGuid(),
                orderNumber: orderNumber ?? Guid.NewGuid().GetHashCode(),
                orderDate: orderDate ?? DateTime.UtcNow,
                customerName: customerName ?? Guid.NewGuid().ToString(),
                customerSurname: customerSurname ?? Guid.NewGuid().ToString(),
                items: items ?? MoreEnumerable.Return(TestData.Create.OrderItemDto()).ToList());
        }
    }
}
