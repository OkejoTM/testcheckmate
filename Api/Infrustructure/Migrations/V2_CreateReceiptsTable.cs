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
            .WithColumn("processing_state").AsString().NotNullable();
    }

    public override void Down() => Delete.Table("receipts");
}

