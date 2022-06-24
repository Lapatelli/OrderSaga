using System;

namespace OrderSaga.Contracts
{
    public class ChangeOrderStatus
    {
        public ChangeOrderStatus(
            int orderNumber,
            OrderStatus status,
            DateTime? updatedDate)
        {
            OrderNumber = orderNumber;
            Status = status;
            UpdatedDate = updatedDate;
        }

        public int OrderNumber { get; }

        public OrderStatus Status { get; }

        public DateTime? UpdatedDate { get; }
    }
}
