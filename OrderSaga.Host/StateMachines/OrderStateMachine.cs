﻿using MassTransit;
using OrderSaga.Contracts;
using OrderSaga.Contracts.Dto;
using OrderSaga.Host.Entities;
using System;
using System.Linq;
using static OrderSaga.Host.StateMachines.OrderStatusStatesConstants;

namespace OrderSaga.Host.StateMachines
{
    public class OrderStateMachine : MassTransitStateMachine<Order>
    {
        public OrderStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(() => OrderCreated, x => x.CorrelateById(m => m.Message.OrderId));
            Event(() => OrderStatusChanged, x => x.CorrelateBy<int>(m => m.OrderNumber, t => t.Message.OrderNumber));
            HandleOrderRequestedEvent();

            HandleInitialState();
            HandleAwaitingPackingState();
            HandlePackedState();
            HandleShippedState();
            HandleCancelledState();
        }

        public State AwaitingPacking { get; set; }

        public State Packed { get; set; }

        public State Shipped { get; set; }

        public State Cancelled { get; set; }

        public Event<OrderCreated> OrderCreated { get; set; }

        public Event<OrderStatusChanged> OrderStatusChanged { get; set; }

        public Event<CheckOrder> OrderRequested { get; set; }

        private void HandleInitialState()
        {
            Initially(
                When(OrderCreated)
                    .Then(ConfigureBehaviourForOrderCreatedEvent())
                    .TransitionTo(AwaitingPacking));
        }

        private void HandleOrderRequestedEvent()
        {
            Event(() => OrderRequested, x =>
            {
                x.CorrelateBy<int>(m => m.OrderNumber, t => t.Message.OrderNumber);
                x.OnMissingInstance(m => m.ExecuteAsync(async context =>
                {
                    if (context.RequestId.HasValue)
                    {
                        await context.RespondAsync(new OrderNotFound(context.Message.OrderNumber));
                    }
                }));
            });

            DuringAny(
                When(OrderRequested)
                    .RespondAsync(x => x.Init<OrderDto>(CreateOrderDto(x.Saga))));
        }

        private void HandleAwaitingPackingState()
        {
            During(AwaitingPacking,
                When(
                    OrderStatusChanged,
                    context => AwaitingPackingState.AwaitingPackingStateAccepted.Contains(context.Message.Status))
                    .Then(ConfigureBehaviourForStatusChangedEventInAwaitingPackingState())
                    .TransitionTo(Packed)
                    .Publish(ConfigureStatusSubmittedMessage(OrderStatus.AwaitingPacking)));

            During(AwaitingPacking,
                When(
                    OrderStatusChanged,
                    context => AwaitingPackingState.AwaitingPackingStateRejected.Contains(context.Message.Status))
                    .Publish(ConfigureStatusRejectedMessage()));
        }

        private void HandlePackedState()
        {
            During(Packed,
                When(
                    OrderStatusChanged,
                    context => PackedState.PackedStateAccepted.Contains(context.Message.Status))
                    .Then(ConfigureBehaviourForStatusChangedEventInPackedState())
                    .TransitionTo(Shipped)
                    .Publish(ConfigureStatusSubmittedMessage(OrderStatus.Packed)));

            During(Packed,
                When(
                    OrderStatusChanged,
                    context => PackedState.PackedStateRejected.Contains(context.Message.Status))
                    .Publish(ConfigureStatusRejectedMessage()));
        }


        private void HandleShippedState()
        {
            During(Shipped,
                When(
                    OrderStatusChanged,
                    context => ShippedState.ShippedStateRejected.Contains(context.Message.Status))
                    .Publish(ConfigureStatusRejectedMessage()));

        }

        private void HandleCancelledState()
        {
            DuringAny(
                When(
                    OrderStatusChanged,
                    context => CancelledState.CancelledStateAccepted.Contains(context.Message.Status))
                    .TransitionTo(Cancelled));

            During(Cancelled,
                Ignore(OrderStatusChanged),
                Ignore(OrderCreated));
        }

        private static Action<BehaviorContext<Order, OrderCreated>> ConfigureBehaviourForOrderCreatedEvent()
        {
            return context =>
            {
                context.Saga.OrderNumber = context.Message.OrderNumber;
                context.Saga.OrderDate = context.Message.OrderDate;
                context.Saga.CustomerName = context.Message.CustomerName;
                context.Saga.CustomerSurname = context.Message.CustomerSurname;
                context.Saga.UpdatedDate = DateTime.UtcNow;

                var items = context.Message.Items
                    .Select(i => OrderItem.Create(i.Sku, i.Quantity, i.Price, context.Saga))
                    .ToList();

                context.Saga.Items = items;
            };
        }

        private static Action<BehaviorContext<Order, OrderStatusChanged>>
            ConfigureBehaviourForStatusChangedEventInAwaitingPackingState() =>
                context =>
                    context.Saga.UpdatedDate = DateTime.UtcNow;

        private static Action<BehaviorContext<Order, OrderStatusChanged>>
            ConfigureBehaviourForStatusChangedEventInPackedState() =>
                context =>
                {
                    context.Saga.UpdatedDate = DateTime.UtcNow;
                    context.Saga.ShippedDate = DateTime.UtcNow;
                };


        private static EventMessageFactory<Order, OrderStatusChanged, OrderStatusChangedSubmitted>
            ConfigureStatusSubmittedMessage(OrderStatus previousOrderStatus) =>
                context =>
                    new OrderStatusChangedSubmitted(
                        orderNumber: context.Saga.OrderNumber,
                        previousOrderStatus: previousOrderStatus,
                        currentOrderStatus: context.Message.Status);

        private static EventMessageFactory<Order, OrderStatusChanged, OrderStatusChangedRejected>
            ConfigureStatusRejectedMessage() =>
                context =>
                    new OrderStatusChangedRejected(
                        orderNumber: context.Saga.OrderNumber,
                        currentOrderStatus: Enum.Parse<OrderStatus>(context.Saga.CurrentState),
                        intendedOrderStatus: context.Message.Status);

        private static OrderDto CreateOrderDto(Order order)
        {
            var items = order.Items
                .Select(i => new OrderItemDto { Sku = i.Sku, Price = i.Price, Quantity = i.Quantity })
                .ToList();

            return new OrderDto(order.OrderNumber, order.CurrentState, order.CustomerName, order.CustomerSurname, items);
        }
    }
}
