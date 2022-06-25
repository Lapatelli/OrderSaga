namespace OrderSaga.Contracts
{
    public class OrderStatusChangedSubmitted
    {
        public OrderStatusChangedSubmitted(
            int orderNumber,
            OrderStatus previousOrderStatus,
            OrderStatus currentOrderStatus)
        {
            OrderNumber = orderNumber;
            PreviousOrderStatus = previousOrderStatus;
            CurrentOrderStatus = currentOrderStatus;
        }

        public int OrderNumber { get; }

        public OrderStatus PreviousOrderStatus { get; }

        public OrderStatus CurrentOrderStatus { get; }
    }
}
