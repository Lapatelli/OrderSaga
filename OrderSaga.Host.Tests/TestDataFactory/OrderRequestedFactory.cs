using OrderSaga.Contracts;
using System;

namespace OrderSaga.Host.Tests.TestDataFactory
{
    public static class OrderRequestedFactory
    {
        public static OrderRequested OrderRequested(
            this ITestDataFactory factory,
            int? orderNumber = null) => new OrderRequested(
                orderNumber: orderNumber ?? Guid.NewGuid().GetHashCode());
    }
}
