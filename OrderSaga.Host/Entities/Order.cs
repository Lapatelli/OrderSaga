using MassTransit;
using System;
using System.Collections.Generic;

namespace OrderSaga.Host.Entities
{
    public class Order : SagaStateMachineInstance, ISagaVersion
    {
        public Guid CorrelationId { get; set; }

        public string CurrentState { get; set; }

        public int OrderNumber { get; set; }

        public DateTime? OrderDate { get; set; }

        public string CustomerName { get; set; }

        public string CustomerSurname { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public DateTime? ShippedDate { get; set; }

        public int Version { get; set; }

        public ICollection<OrderItem> Items { get; set; }
    }
}
