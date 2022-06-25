namespace OrderSaga.Contracts
{
    public class CheckOrder
    {
        public CheckOrder(int orderNumber)
        {
            OrderNumber = orderNumber;
        }

        public int OrderNumber { get; }
    }
}
