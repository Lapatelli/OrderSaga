using System.Collections.Generic;

namespace OrderSaga.Contracts.Dto
{
    public class OrderDto
    {
        public OrderDto(
            int orderNumber,
            string orderStatus,
            string customerName,
            string customerSurname,
            ICollection<OrderItemDto> items)
        {
            OrderNumber = orderNumber;
            OrderStatus = orderStatus;
            CustomerName = customerName;
            CustomerSurname = customerSurname;
            Items = items;
        }

        public OrderDto()
        {
        }

        public int OrderNumber { get; set; }

        public string OrderStatus { get; set;  }

        public string CustomerName { get; set;  }

        public string CustomerSurname { get; set;  }

        public ICollection<OrderItemDto> Items { get; set;  }
    }
}
