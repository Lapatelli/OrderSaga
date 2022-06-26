using NUnit.Framework;
using OrderSaga.Contracts;
using OrderSaga.Host.Tests.TestDataFactory;
using System.Threading.Tasks;

namespace OrderSaga.Host.Tests.StateMachines
{
    [TestFixture]
    public class OrderStateMachineTests : OrderStateMachineTestsFixture
    {
        [Test]
        public async Task OrderCreated_ProvidedAllNecessaryData_ShouldCreateOrder()
        {
            var message = TestData.Create.OrderCreated();

            await Harness.Bus.Publish(message);

            await AssertSagaConsumedMessage<OrderCreated>();
            AssertOrderProperties(message, OrderStatus.AwaitingPacking);
        }

        [Test]
        public async Task OrderStatusChanged_FromAwaitingPackingToPacked_OrderStateIsPacked()
        {
            var orderCreatedMessage = TestData.Create.OrderCreated();
            var statusChangedMessage = TestData.Create.OrderStatusChanged(orderCreatedMessage.OrderNumber, OrderStatus.Packed);
            await DeliverOrderFromInitialToAwaitingPackingState(orderCreatedMessage);

            await SendEndpoint.Send(statusChangedMessage);
            await WaitForState(orderCreatedMessage.OrderId, StateMachine.Packed);

            await AssertSagaConsumedMessage<OrderStatusChanged>();
            AssertOrderProperties(orderCreatedMessage, OrderStatus.Packed, statusChangedMessage.UpdatedDate);
        }

        [TestCase(OrderStatus.AwaitingPacking)]
        [TestCase(OrderStatus.Shipped)]
        public async Task OrderStatusChanged_InvalidStatusForUpdateProvidedForAwaitingPackingOrder_OrderStateIsNotChanged(
            OrderStatus providedStatus)
        {
            var orderCreatedMessage = TestData.Create.OrderCreated();
            var statusChangedMessage = TestData.Create.OrderStatusChanged(orderCreatedMessage.OrderNumber, providedStatus);
            await DeliverOrderFromInitialToAwaitingPackingState(orderCreatedMessage);

            await SendEndpoint.Send(statusChangedMessage);

            await AssertSagaConsumedMessage<OrderStatusChanged>();
            AssertOrderProperties(orderCreatedMessage, OrderStatus.AwaitingPacking, statusChangedMessage.UpdatedDate);
        }

        [Test]
        public async Task OrderStatusChanged_FromPackedToShipped_OrderStateIsShipped()
        {
            var orderCreatedMessage = TestData.Create.OrderCreated();
            var statusChangedMessage = TestData.Create.OrderStatusChanged(orderCreatedMessage.OrderNumber, OrderStatus.Shipped);
            await DeliverOrderFromInitialToPackedState(orderCreatedMessage);

            await SendEndpoint.Send(statusChangedMessage);
            await WaitForState(orderCreatedMessage.OrderId, StateMachine.Shipped);

            await AssertSagaConsumedMessage<OrderStatusChanged>();
            AssertOrderProperties(
                orderCreatedMessage,
                OrderStatus.Shipped,
                statusChangedMessage.UpdatedDate,
                statusChangedMessage.UpdatedDate);
        }

        [TestCase(OrderStatus.AwaitingPacking)]
        [TestCase(OrderStatus.Packed)]
        public async Task OrderStatusChanged_InvalidStatusForUpdateProvidedForPackedOrder_OrderStateIsNotChanged(
            OrderStatus providedStatus)
        {
            var orderCreatedMessage = TestData.Create.OrderCreated();
            var statusChangedMessage = TestData.Create.OrderStatusChanged(orderCreatedMessage.OrderNumber, providedStatus);
            await DeliverOrderFromInitialToPackedState(orderCreatedMessage);

            await SendEndpoint.Send(statusChangedMessage);

            await AssertSagaConsumedMessage<OrderStatusChanged>();
            AssertOrderProperties(orderCreatedMessage, OrderStatus.Packed, statusChangedMessage.UpdatedDate);
        }

        [TestCase(OrderStatus.AwaitingPacking)]
        [TestCase(OrderStatus.Packed)]
        [TestCase(OrderStatus.Shipped)]
        public async Task OrderStatusChanged_InvalidStatusForUpdateProvidedForShippedOrder_OrderStateIsNotChanged(
            OrderStatus providedStatus)
        {
            var orderCreatedMessage = TestData.Create.OrderCreated();
            var statusChangedMessage = TestData.Create.OrderStatusChanged(orderCreatedMessage.OrderNumber, providedStatus);
            await DeliverOrderFromInitialToShippedState(orderCreatedMessage);

            await SendEndpoint.Send(statusChangedMessage);

            await AssertSagaConsumedMessage<OrderStatusChanged>();
            AssertOrderProperties(orderCreatedMessage, OrderStatus.Shipped, statusChangedMessage.UpdatedDate);
        }

        [Test]
        public async Task OrderStatusChanged_FromAwaitingPackingToCancelled_OrderStatusChanged()
        {
            var orderCreatedMessage = TestData.Create.OrderCreated();
            var statusChangedMessage = TestData.Create.OrderStatusChanged(orderCreatedMessage.OrderNumber, OrderStatus.Cancelled);
            await DeliverOrderFromInitialToAwaitingPackingState(orderCreatedMessage);

            await SendEndpoint.Send(statusChangedMessage);

            await AssertSagaConsumedMessage<OrderStatusChanged>();
            await WaitForState(orderCreatedMessage.OrderId, StateMachine.Cancelled);
            AssertOrderProperties(orderCreatedMessage, OrderStatus.Cancelled);
        }

        [Test]
        public async Task OrderStatusChanged_FromPackedToCancelled_OrderStatusChanged()
        {
            var orderCreatedMessage = TestData.Create.OrderCreated();
            var statusChangedMessage = TestData.Create.OrderStatusChanged(orderCreatedMessage.OrderNumber, OrderStatus.Cancelled);
            await DeliverOrderFromInitialToPackedState(orderCreatedMessage);

            await SendEndpoint.Send(statusChangedMessage);

            await AssertSagaConsumedMessage<OrderStatusChanged>();
            await WaitForState(orderCreatedMessage.OrderId, StateMachine.Cancelled);
            AssertOrderProperties(orderCreatedMessage, OrderStatus.Cancelled);
        }

        [Test]
        public async Task OrderStatusChanged_FromShippedToCancelled_OrderStatusChanged()
        {
            var orderCreatedMessage = TestData.Create.OrderCreated();
            var statusChangedMessage = TestData.Create.OrderStatusChanged(orderCreatedMessage.OrderNumber, OrderStatus.Cancelled);
            await DeliverOrderFromInitialToShippedState(orderCreatedMessage);

            await SendEndpoint.Send(statusChangedMessage);

            await AssertSagaConsumedMessage<OrderStatusChanged>();
            await WaitForState(orderCreatedMessage.OrderId, StateMachine.Cancelled);
            AssertOrderProperties(orderCreatedMessage, OrderStatus.Cancelled);
        }
    }
}