using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using OrderSaga.Contracts;
using OrderSaga.Host.Entities;

namespace OrderSaga.Host.Mapping
{
    public class OrderItemMap : ClassMapping<OrderItem>
    {
        public OrderItemMap()
        {
            Id(x => x.Sku);
            Property(x => x.Price);
            Property(x => x.Quantity);
            ManyToOne(x => x.OrderCorrelationId, m => { m.Column("OrderCorrelationId"); m.Cascade(Cascade.All); });
            Table("OrderItems");
        }
    }
}
