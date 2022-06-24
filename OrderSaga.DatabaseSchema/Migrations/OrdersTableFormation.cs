using FluentMigrator;

namespace OrderSaga.DatabaseSchema.Migrations
{
    [Migration(202206220001)]
    public class OrdersTableFormation : Migration
    {
        public override void Up()
        {
            Create.Table("Orders")
                .WithColumn("CorrelationId").AsGuid().PrimaryKey()
                .WithColumn("CurrentState").AsString().Nullable()
                .WithColumn("OrderNumber").AsInt32().Nullable()
                .WithColumn("OrderDate").AsDateTime().Nullable()
                .WithColumn("CustomerName").AsString().Nullable()
                .WithColumn("CustomerSurname").AsString().Nullable()
                .WithColumn("UpdatedDate").AsDateTime().Nullable()
                .WithColumn("ShippedDate").AsDateTime().Nullable()
                .WithColumn("Version").AsInt32();
        }

        public override void Down()
        {
        }
    }
}
