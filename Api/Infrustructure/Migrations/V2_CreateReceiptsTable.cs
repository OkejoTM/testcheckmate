namespace Infrustructure.Migrations;

using Domain;
using FluentMigrator;

[Migration(2)]
public class V2_CreateReceiptsTable : Migration {
    public override void Up() {
        Create.Table("receipts")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("created_at").AsDateTime().Nullable().WithDefault(SystemMethods.CurrentDateTime)
            .WithColumn("updated_at").AsDateTime().Nullable().WithDefault(SystemMethods.CurrentDateTime)
            .WithColumn("file_id").AsGuid().ForeignKey("stored_files", "id")
            .WithColumn("comment").AsString().NotNullable()
            .WithColumn("processing_state").AsString().NotNullable()

            .WithColumn("uploaded_by_user_id").AsInt32().NotNullable()
            .WithColumn("operation_type").AsString().Nullable()
            .WithColumn("category_by_store").AsString().Nullable()
            .WithColumn("category_by_price").AsString().Nullable()

            .WithColumn("date").AsString().Nullable()
            .WithColumn("time").AsString().Nullable()
            .WithColumn("total").AsString().Nullable()
            .WithColumn("fiscal_number").AsString().Nullable()
            .WithColumn("fiscal_document").AsString().Nullable()
            .WithColumn("fiscal_sign").AsString().Nullable()
            .WithColumn("inn").AsString().Nullable()
            .WithColumn("receipt_number").AsString().Nullable()
            .WithColumn("store_name").AsString().Nullable()
            .WithColumn("vat_amount").AsString().Nullable();
    }

    public override void Down() => Delete.Table("receipts");
}

