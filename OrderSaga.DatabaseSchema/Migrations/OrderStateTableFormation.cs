using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderSaga.DatabaseSchema.Migrations
{
    [Migration(202206220009)]
    public class OrderStateTableFormation : Migration
    {
        public override void Up()
        {
            Create.Table("OrderState")
                .WithColumn("CorrelationId").AsGuid().PrimaryKey()
                .WithColumn("CurrentState").AsString().Nullable()
                .WithColumn("IntState").AsInt32().Nullable()
                .WithColumn("Version").AsInt32().Nullable()
                .WithColumn("OrderNumber").AsInt32().Nullable()
                .WithColumn("OrderDate").AsDateTime().Nullable()
                .WithColumn("UpdatedDate").AsDateTime().Nullable();
        }

        public override void Down()
        {
        }
    }
}
