using MassTransit.NHibernateIntegration;
using NHibernate.Mapping.ByCode;
using OrderSaga.Contracts;
using OrderSaga.Host.Entities;

namespace OrderSaga.Host.Mapping
{
    public class OrderMap : SagaClassMapping<Order>
    {
        public OrderMap()
        {
            Property(x => x.CurrentState, x => x.Length(64));
            Property(x => x.OrderNumber);
            Property(x => x.OrderDate);
            Property(x => x.CustomerName);
            Property(x => x.CustomerSurname);
            Property(x => x.UpdatedDate);
            Property(x => x.ShippedDate);
            Property(x => x.Version);
            Bag(x => x.Items, map => { map.Key(k => k.Column("OrderCorrelationId")); map.Inverse(true); map.Cascade(Cascade.All); }, rel => rel.OneToMany());
            Table("Orders");
        }
    }
}
