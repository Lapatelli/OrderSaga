using System;

namespace OrderSaga.Contracts.Dto
{
    public class OrderItemDto
    {
        public long Sku { get; set; }

        public int Price { get; set; }

        public int Quantity { get; set; }
    }
}
