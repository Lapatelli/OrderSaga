using OrderSaga.Contracts;
using System.Collections.Generic;

namespace OrderSaga.Host.StateMachines
{
    internal static class OrderStatusStatesConstants
    {
        internal static class AwaitingPackingState
        {
            internal static ICollection<OrderStatus> AwaitingPackingStateAccepted => new List<OrderStatus>
            {
                OrderStatus.Packed
            };

            internal static ICollection<OrderStatus> AwaitingPackingStateRejected => new List<OrderStatus>
            {
                OrderStatus.AwaitingPacking,
                OrderStatus.Shipped
            };
        }

        internal static class PackedState
        {
            internal static ICollection<OrderStatus> PackedStateAccepted => new List<OrderStatus>
            {
                OrderStatus.Shipped
            };

            internal static ICollection<OrderStatus> PackedStateRejected => new List<OrderStatus>
            {
                OrderStatus.AwaitingPacking,
                OrderStatus.Packed
            };
        }

        internal static class ShippedState
        {
            internal static ICollection<OrderStatus> ShippedStateRejected => new List<OrderStatus>
            {
                OrderStatus.AwaitingPacking,
                OrderStatus.Packed,
                OrderStatus.Shipped
            };
        }

        internal static class CancelledState
        {
            internal static ICollection<OrderStatus> CancelledStateAccepted => new List<OrderStatus>
            {
                OrderStatus.Cancelled
            };
        }
    }
}
