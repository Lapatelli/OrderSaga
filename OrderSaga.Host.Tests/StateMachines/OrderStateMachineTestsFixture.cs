using FluentAssertions;
using MassTransit;
using MassTransit.Testing;
using NUnit.Framework;
using OrderSaga.Contracts;
using OrderSaga.Host.Entities;
using OrderSaga.Host.StateMachines;
using OrderSaga.Host.Tests.TestDataFactory;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OrderSaga.Host.Tests.StateMachines
{
    public class OrderStateMachineTestsFixture
    {
        private InMemoryTestHarness _harness;
        private OrderStateMachine _stateMachine;
        private ISagaStateMachineTestHarness<OrderStateMachine, Order> _sagaHarness;

        protected InMemoryTestHarness Harness => _harness;

        protected OrderStateMachine StateMachine => _stateMachine;

        protected ISendEndpoint SendEndpoint => _harness.InputQueueSendEndpoint;


        [SetUp]
        public async Task Setup()
        {
            _harness = new InMemoryTestHarness();
            _stateMachine = new OrderStateMachine();
            _sagaHarness = _harness.StateMachineSaga<Order, OrderStateMachine>(_stateMachine);
            await _harness.Start();
        }

        [TearDown]
        public async Task TearDown()
        {
            await _harness.Stop();
        }

        protected async Task DeliverOrderFromInitialToAwaitingPackingState(OrderCreated orderMessage)
        {
            await SendEndpoint.Send(orderMessage);
            await WaitForState(orderMessage.OrderId, _stateMachine.AwaitingPacking);
        }

        protected async Task DeliverOrderFromInitialToPackedState(OrderCreated orderMessage)
        {
            var packedStatusMessage = TestData.Create.OrderStatusChanged(orderMessage.OrderNumber, OrderStatus.Packed);

            await SendEndpoint.Send(orderMessage);
            await WaitForState(orderMessage.OrderId, StateMachine.AwaitingPacking);
            await SendEndpoint.Send(packedStatusMessage);
            await WaitForState(orderMessage.OrderId, StateMachine.Packed);
        }

        protected async Task DeliverOrderFromInitialToShippedState(OrderCreated orderMessage)
        {
            var packedStatusMessage = TestData.Create.OrderStatusChanged(orderMessage.OrderNumber, OrderStatus.Packed);
            var shippedStatusMessage = TestData.Create.OrderStatusChanged(orderMessage.OrderNumber, OrderStatus.Shipped);

            await SendEndpoint.Send(orderMessage);
            await WaitForState(orderMessage.OrderId, StateMachine.AwaitingPacking);
            await SendEndpoint.Send(packedStatusMessage);
            await WaitForState(orderMessage.OrderId, StateMachine.Packed);
            await SendEndpoint.Send(shippedStatusMessage);
            await WaitForState(orderMessage.OrderId, StateMachine.Shipped);
        }

        protected async Task<ISagaInstance<Order>> WaitForState(Guid correlationId, State state)
        {
            var matchingSagaIds = await _sagaHarness.Match(
                x => x.CorrelationId == correlationId && x.CurrentState == state.Name,
                TimeSpan.FromSeconds(10));

            var sagaInstance = _sagaHarness.Sagas
                .Select(i => i.Saga.CorrelationId == correlationId).FirstOrDefault();

            return sagaInstance;
        }

        protected async Task AssertSagaConsumedMessage<T>() where T : class
        {
            Assert.That(await _harness.Consumed.Any<T>());
            Assert.That(await _sagaHarness.Consumed.Any<T>());
        }

        protected void AssertOrderProperties(
            OrderCreated message,
            OrderStatus expectedOrderStatus,
            DateTime? updatedDate = null,
            DateTime? shippedDate = null)
        {
            var order = _sagaHarness.Sagas
                .Select(m => m.CorrelationId == message.OrderId)
                .FirstOrDefault()?.Saga;

            order.Should().NotBeNull();
            order.OrderNumber.Should().Be(message.OrderNumber);
            Enum.Parse<OrderStatus>(order.CurrentState).Should().Be(expectedOrderStatus);
            order.CustomerName.Should().BeEquivalentTo(message.CustomerName);
            order.CustomerSurname.Should().BeEquivalentTo(message.CustomerSurname);
            order.Items.Should().ContainSingle()
                .Which.Sku.Should().Be(message.Items.First().Sku);

            if (updatedDate != null)
            {
                order.UpdatedDate?.Date.Should().Be(updatedDate.Value.Date);
            }

            if (shippedDate != null)
            {
                order.ShippedDate?.Date.Should().Be(shippedDate.Value.Date);
            }
        }
    }
}
