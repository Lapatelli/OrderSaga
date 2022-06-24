namespace OrderSaga.Host.Entities
{
    public class OrderItem
    {
        public OrderItem()
        {
        }

        private OrderItem(
            long sku,
            int price,
            int quantity,
            Order orderCorrelationId)
        {
            Sku = sku;
            Price = price;
            Quantity = quantity;
            OrderCorrelationId = orderCorrelationId;
        }

        public virtual long Sku { get; set; }

        public virtual int Price { get; set; }

        public virtual int Quantity { get; set; }

        public virtual Order OrderCorrelationId { get; set; }


        public static OrderItem Create(long sku, int price, int quantity, Order orderCorrelationId)
            => new OrderItem(sku, price, quantity, orderCorrelationId);
    }
}
