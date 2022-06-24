using OrderSaga.Contracts.Dto;
using System;
using System.Collections.Generic;

namespace OrderSaga.Contracts
{
    public class CreateOrder
    {
        public CreateOrder(
            Guid orderId,
            DateTime orderDate,
            int orderNumber,
            string customerName,
            string customerSurname,
            ICollection<OrderItemDto> items)
        {
            OrderId = orderId;
            OrderDate = orderDate;
            OrderNumber = orderNumber;
            CustomerName = customerName;
            CustomerSurname = customerSurname;
            Items = items;
        }

        public Guid OrderId { get; }

        public DateTime OrderDate { get; }

        public int OrderNumber { get; }

        public string CustomerName { get; }

        public string CustomerSurname { get; }

        public ICollection<OrderItemDto> Items { get; }
    }
}
