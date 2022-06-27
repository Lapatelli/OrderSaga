namespace OrderSaga.Contracts
{
    public class OrderStatusChangedSubmitted
    {
        public OrderStatusChangedSubmitted(
            int orderNumber,
            OrderStatus currentOrderStatus)
        {
            OrderNumber = orderNumber;
            CurrentOrderStatus = currentOrderStatus;
        }

        public int OrderNumber { get; }

        public OrderStatus CurrentOrderStatus { get; }
    }
}
