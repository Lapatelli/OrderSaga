using System;

namespace OrderSaga.Contracts
{
    public class OrderCreationRejected
    {
        public OrderCreationRejected(
            Guid orderId,
            DateTime orderDate,
            string reason)
        {
            OrderId = orderId;
            OrderDate = orderDate;
            Reason = reason;
        }

        public Guid OrderId { get; }

        public DateTime OrderDate { get; }

        public string Reason { get; }
    }
}
