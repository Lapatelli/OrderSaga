namespace OrderSaga.Contracts
{
    public class OrderRequested
    {
        public OrderRequested(int orderNumber)
        {
            OrderNumber = orderNumber;
        }

        public int OrderNumber { get; }
    }
}
