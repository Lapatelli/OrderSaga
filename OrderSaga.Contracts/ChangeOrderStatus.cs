﻿using System;

namespace OrderSaga.Contracts
{
    public class ChangeOrderStatus
    {
        public ChangeOrderStatus(
            Guid orderId,
            OrderStatus status)
        {
            OrderId = orderId;
            Status = status;
        }

        public Guid OrderId { get; }

        public OrderStatus Status { get; }
    }
}
