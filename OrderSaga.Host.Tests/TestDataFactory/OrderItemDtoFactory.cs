using OrderSaga.Contracts.Dto;
using System;

namespace OrderSaga.Host.Tests.TestDataFactory
{
    public static class OrderItemDtoFactory
    {
        public static OrderItemDto OrderItemDto(
            this ITestDataFactory factory,
            long? sku = null,
            int? price = null,
            int? quantity = null)
        {
            return new OrderItemDto
               {
                   Sku = sku ?? Guid.NewGuid().GetHashCode(),
                   Price = price ?? Guid.NewGuid().GetHashCode(),
                   Quantity = quantity ?? Guid.NewGuid().GetHashCode()
               };
        }
    }
}
