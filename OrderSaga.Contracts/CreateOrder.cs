using OrderSaga.Contracts.Dto;
using System;
using System.Collections.Generic;

namespace OrderSaga.Contracts
{
    public class CreateOrder
    {
        public CreateOrder(
            DateTime orderDate,
            string customerName,
            string customerSurname,
            ICollection<OrderItemDto> items)
        {
            OrderDate = orderDate;
            CustomerName = customerName;
            CustomerSurname = customerSurname;
            Items = items;
        }


        public DateTime OrderDate { get; }

        public string CustomerName { get; }

        public string CustomerSurname { get; }

        public ICollection<OrderItemDto> Items { get; }
    }
}
