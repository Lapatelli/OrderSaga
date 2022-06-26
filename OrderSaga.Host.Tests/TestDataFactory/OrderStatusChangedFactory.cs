using OrderSaga.Contracts;
using System;

namespace OrderSaga.Host.Tests.TestDataFactory
{
    public static class OrderStatusChangedFactory
    {
        public static OrderStatusChanged OrderStatusChanged(
            this ITestDataFactory factory,
            int? orderNumber = null,
            OrderStatus? orderStatus = null,
            DateTime? updatedDate = null)
        {
            return new OrderStatusChanged(
                orderNumber: orderNumber ?? Guid.NewGuid().GetHashCode(),
                status: orderStatus ?? OrderStatus.Initial,
                updatedDate: updatedDate ?? DateTime.UtcNow);
        }
    }
}
