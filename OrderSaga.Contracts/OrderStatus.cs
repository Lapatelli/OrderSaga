namespace OrderSaga.Contracts
{
    public enum OrderStatus
    {
        Initial,
        AwaitingPacking,
        Packed,
        Shipped,
        Cancelled
    }
}
