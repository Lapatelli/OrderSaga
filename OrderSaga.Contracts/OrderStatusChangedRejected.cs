namespace OrderSaga.Contracts
{
    public class OrderStatusChangedRejected
    {
        public OrderStatusChangedRejected(
            int orderNumber,
            OrderStatus currentOrderStatus,
            OrderStatus intendedOrderStatus)
        {
            OrderNumber = orderNumber;
            CurrentOrderStatus = currentOrderStatus;
            IntendedOrderStatus = intendedOrderStatus;
        }

        public int OrderNumber { get; }

        public OrderStatus CurrentOrderStatus { get; }

        public OrderStatus IntendedOrderStatus { get; }
    }
}
