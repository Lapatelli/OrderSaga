using FluentMigrator;

namespace OrderSaga.DatabaseSchema.Migrations
{
    [Migration(202206221002)]
    public class OrderItemsTableFormation : Migration
    {
        public override void Up()
        {
            Create.Table("OrderItems")
                .WithColumn("Sku").AsInt64().PrimaryKey()
                .WithColumn("Price").AsInt32()
                .WithColumn("Quantity").AsInt32()
                .WithColumn("OrderCorrelationId").AsGuid().ForeignKey("Orders", "CorrelationId").Nullable();
        }

        public override void Down()
        {
        }
    }
}
