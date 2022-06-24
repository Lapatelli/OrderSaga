using System;

namespace OrderSaga.Contracts
{
    public class OrderStatusChangingAccepted
    {
        public OrderStatusChangingAccepted(
            Guid orderId,
            OrderStatus status,
            DateTime? updatedDate)
        {
            OrderId = orderId;
            Status = status;
            UpdatedDate = updatedDate;
        }

        public Guid OrderId { get; }

        public OrderStatus Status { get; }

        public DateTime? UpdatedDate { get; }
    }
}
