using OrderSaga.Contracts.Dto;
using System;
using System.Collections.Generic;

namespace OrderSaga.Contracts
{
    public class OrderCreated
    {
        public OrderCreated(
            Guid orderId,
            int orderNumber,
            DateTime orderDate,
            string customerName,
            string customerSurname,
            ICollection<OrderItemDto> items)
        {
            OrderId = orderId;
            OrderNumber = orderNumber;
            OrderDate = orderDate;
            CustomerName = customerName;
            CustomerSurname = customerSurname;
            Items = items;
        }

        public Guid OrderId { get; }

        public int OrderNumber { get; }

        public DateTime OrderDate { get; }

        public string CustomerName { get; }

        public string CustomerSurname { get; }

        public ICollection<OrderItemDto> Items { get; }
    }
}
