namespace Infrustructure.Migrations;

using FluentMigrator;

[Migration(1)]
public class V1_CreateStoredFilesTable : Migration {
    public override void Up() {
        Create.Table("stored_files")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("created_at").AsDateTime().Nullable().WithDefault(SystemMethods.CurrentDateTime)
            .WithColumn("updated_at").AsDateTime().Nullable().WithDefault(SystemMethods.CurrentDateTime)
            .WithColumn("path").AsString(255).NotNullable()
            .WithColumn("name").AsString(255).NotNullable()
            .WithColumn("mime_type").AsString(100).NotNullable();
    }

    public override void Down() => Delete.Table("stored_files");
}
