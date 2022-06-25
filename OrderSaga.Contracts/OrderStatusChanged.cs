using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel.DataAnnotations;

namespace OrderSaga.Contracts
{
    public class OrderStatusChanged
    {
        public OrderStatusChanged(
            int orderNumber,
            OrderStatus status,
            DateTime? updatedDate)
        {
            OrderNumber = orderNumber;
            Status = status;
            UpdatedDate = updatedDate;
        }

        public int OrderNumber { get; }

        [EnumDataType(typeof(OrderStatus))]
        [JsonConverter(typeof(StringEnumConverter))]
        public OrderStatus Status { get; }

        public DateTime? UpdatedDate { get; }
    }
}
