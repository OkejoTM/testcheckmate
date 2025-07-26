namespace Infrustructure.Migrations;

using Domain;
using FluentMigrator;

[Migration(3)]
public class V3_CreateReceiptItemsTable : Migration {
    public override void Up() {
        Create.Table("receipt_items")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("created_at").AsDateTime().Nullable().WithDefault(SystemMethods.CurrentDateTime)
            .WithColumn("updated_at").AsDateTime().Nullable().WithDefault(SystemMethods.CurrentDateTime)
            .WithColumn("product_name").AsString().NotNullable()
            .WithColumn("quantity").AsString().NotNullable()
            .WithColumn("unit").AsString().NotNullable()
            .WithColumn("price_per_unit").AsString().NotNullable()
            .WithColumn("total_price").AsString().NotNullable()

            .WithColumn("receipt_id").AsGuid().ForeignKey("receipts", "id");
    }

    public override void Down() => Delete.Table("receipt_items");
}
