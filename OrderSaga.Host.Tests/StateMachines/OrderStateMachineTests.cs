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
            var orderMessage = TestData.Create.OrderCreated();
            var statusMessage = TestData.Create.OrderStatusChanged(orderMessage.OrderNumber, OrderStatus.Packed);

            await DeliverOrderFromInitialToAwaitingPackingState(orderMessage);

            await SendEndpoint.Send(statusMessage);
            await WaitForState(orderMessage.OrderId, StateMachine.Packed);

            await AssertSagaConsumedMessage<OrderStatusChanged>();
            AssertOrderProperties(orderMessage, OrderStatus.Packed, statusMessage.UpdatedDate);
        }

        [TestCase(OrderStatus.AwaitingPacking)]
        [TestCase(OrderStatus.Shipped)]
        public async Task OrderStatusChanged_InvalidStatusForUpdateProvidedForAwaitingPackingOrder_OrderStateIsNotChanged(
            OrderStatus providedStatus)
        {
            var orderMessage = TestData.Create.OrderCreated();
            var statusMessage = TestData.Create.OrderStatusChanged(orderMessage.OrderNumber, providedStatus);

            await DeliverOrderFromInitialToAwaitingPackingState(orderMessage);

            await SendEndpoint.Send(statusMessage);

            await AssertSagaConsumedMessage<OrderStatusChanged>();
            AssertOrderProperties(orderMessage, OrderStatus.AwaitingPacking, statusMessage.UpdatedDate);
        }

        [Test]
        public async Task OrderStatusChanged_FromPackedToShipped_OrderStateIsShipped()
        {
            var orderMessage = TestData.Create.OrderCreated();
            var statusMessage = TestData.Create.OrderStatusChanged(orderMessage.OrderNumber, OrderStatus.Shipped);

            await DeliverOrderFromInitialToPackedState(orderMessage);

            await SendEndpoint.Send(statusMessage);
            await WaitForState(orderMessage.OrderId, StateMachine.Shipped);

            await AssertSagaConsumedMessage<OrderStatusChanged>();
            AssertOrderProperties(orderMessage, OrderStatus.Shipped, statusMessage.UpdatedDate, statusMessage.UpdatedDate);
        }

        [TestCase(OrderStatus.AwaitingPacking)]
        [TestCase(OrderStatus.Packed)]
        public async Task OrderStatusChanged_InvalidStatusForUpdateProvidedForPackedOrder_OrderStateIsNotChanged(
            OrderStatus providedStatus)
        {
            var orderMessage = TestData.Create.OrderCreated();
            var statusMessage = TestData.Create.OrderStatusChanged(orderMessage.OrderNumber, providedStatus);

            await DeliverOrderFromInitialToPackedState(orderMessage);

            await SendEndpoint.Send(statusMessage);

            await AssertSagaConsumedMessage<OrderStatusChanged>();
            AssertOrderProperties(orderMessage, OrderStatus.Packed, statusMessage.UpdatedDate);
        }

        [TestCase(OrderStatus.AwaitingPacking)]
        [TestCase(OrderStatus.Packed)]
        [TestCase(OrderStatus.Shipped)]
        public async Task OrderStatusChanged_InvalidStatusForUpdateProvidedForShippedOrder_OrderStateIsNotChanged(
            OrderStatus providedStatus)
        {
            var orderMessage = TestData.Create.OrderCreated();
            var statusMessage = TestData.Create.OrderStatusChanged(orderMessage.OrderNumber, providedStatus);

            await DeliverOrderFromInitialToShippedState(orderMessage);

            await SendEndpoint.Send(statusMessage);

            await AssertSagaConsumedMessage<OrderStatusChanged>();
            AssertOrderProperties(orderMessage, OrderStatus.Shipped, statusMessage.UpdatedDate);
        }

        [Test]
        public async Task OrderStatusChanged_FromAwaitingPackingToCancelled_OrderStatusChanged()
        {
            var orderMessage = TestData.Create.OrderCreated();
            var statusMessage = TestData.Create.OrderStatusChanged(orderMessage.OrderNumber, OrderStatus.Cancelled);

            await DeliverOrderFromInitialToAwaitingPackingState(orderMessage);

            await SendEndpoint.Send(statusMessage);

            await AssertSagaConsumedMessage<OrderStatusChanged>();
            await WaitForState(orderMessage.OrderId, StateMachine.Cancelled);
            AssertOrderProperties(orderMessage, OrderStatus.Cancelled);
        }

        [Test]
        public async Task OrderStatusChanged_FromPackedToCancelled_OrderStatusChanged()
        {
            var orderMessage = TestData.Create.OrderCreated();
            var statusMessage = TestData.Create.OrderStatusChanged(orderMessage.OrderNumber, OrderStatus.Cancelled);

            await DeliverOrderFromInitialToPackedState(orderMessage);

            await SendEndpoint.Send(statusMessage);

            await AssertSagaConsumedMessage<OrderStatusChanged>();
            await WaitForState(orderMessage.OrderId, StateMachine.Cancelled);
            AssertOrderProperties(orderMessage, OrderStatus.Cancelled);
        }

        [Test]
        public async Task OrderStatusChanged_FromShippedToCancelled_OrderStatusChanged()
        {
            var orderMessage = TestData.Create.OrderCreated();
            var statusMessage = TestData.Create.OrderStatusChanged(orderMessage.OrderNumber, OrderStatus.Cancelled);

            await DeliverOrderFromInitialToShippedState(orderMessage);

            await SendEndpoint.Send(statusMessage);

            await AssertSagaConsumedMessage<OrderStatusChanged>();
            await WaitForState(orderMessage.OrderId, StateMachine.Cancelled);
            AssertOrderProperties(orderMessage, OrderStatus.Cancelled);
        }
    }
}