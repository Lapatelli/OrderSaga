using System;

namespace OrderSaga.Contracts
{
    public class OrderCreationRejected
    {
        public OrderCreationRejected(
            Guid orderId,
            DateTime orderDate,
            int orderNumber,
            string reason)
        {
            OrderId = orderId;
            OrderDate = orderDate;
            OrderNumber = orderNumber;
            Reason = reason;
        }

        public Guid OrderId { get; }

        public DateTime OrderDate { get; }

        public int OrderNumber { get; }

        public string Reason { get; }
    }
}
