using MassTransit;
using MoreLinq;
using OrderSaga.Contracts;
using OrderSaga.Host.Entities;
using System;
using System.Linq;

namespace OrderSaga.Host.StateMachines
{
    public class OrderStateMachine : MassTransitStateMachine<Order>
    {
        public OrderStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(() => OrderCreated, x => x.CorrelateById(m => m.Message.OrderId));
            Event(() => OrderStatusChanged, x => x.CorrelateBy<int>(m => m.OrderNumber, t => t.Message.OrderNumber));

            HandleInitialState();
            HandleAwaitingPackingState();
            HandlePackedState();
            HandleCancelledState();
        }

        public State AwaitingPacking { get; set; }

        public State Packed { get; set; }

        public State Shipped { get; set; }

        public State Cancelled { get; set; }

        public Event<OrderCreated> OrderCreated { get; set; }

        public Event<OrderStatusChanged> OrderStatusChanged { get; set; }

        private void HandleInitialState()
        {
            Initially(
                When(OrderCreated)
                    .Then(context =>
                    {
                        context.Saga.OrderNumber = context.Message.OrderNumber;
                        context.Saga.OrderDate = context.Message.OrderDate;
                        context.Saga.CustomerName = context.Message.CustomerName;
                        context.Saga.CustomerSurname = context.Message.CustomerSurname;
                        context.Saga.UpdatedDate = DateTime.UtcNow;

                        var items =  context.Message.Items
                            .Select(i => OrderItem.Create(i.Sku, i.Quantity, i.Price, context.Saga))
                            .ToList();

                        context.Saga.Items = items;
                    })
                    .TransitionTo(AwaitingPacking));
        }

        private void HandleAwaitingPackingState()
        {
            During(AwaitingPacking,
                Ignore(OrderCreated));

            During(AwaitingPacking,
                When(OrderStatusChanged, context => context.Message.Status == OrderStatus.Packed)
                    .Then(context =>
                    {
                        context.Saga.UpdatedDate = DateTime.UtcNow;
                    })
                    .TransitionTo(Packed));
        }

        private void HandlePackedState()
        {
            During(Packed,
                When(OrderStatusChanged, context => context.Message.Status == OrderStatus.Shipped)
                    .Then(context =>
                    {
                        context.Saga.UpdatedDate = DateTime.UtcNow;
                        context.Saga.ShippedDate = DateTime.UtcNow;
                    })
                    .TransitionTo(Shipped));
        }

        private void HandleCancelledState()
        {
            DuringAny(
                When(OrderStatusChanged, context => context.Message.Status == OrderStatus.Cancelled)
                    .TransitionTo(Cancelled));

            During(Cancelled,
                Ignore(OrderStatusChanged),
                Ignore(OrderCreated));
        }
    }
}
