namespace OrderSaga.Contracts
{
    public class OrderNotFound
    {
        public OrderNotFound(int orderNumber)
        {
            OrderNumber = orderNumber;
        }

        public int OrderNumber { get; }
    }
}
