using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrustructure.Configurations;

public class StoredFileConfiguration : IEntityTypeConfiguration<StoredFile> {
    public void Configure(EntityTypeBuilder<StoredFile> builder) {
        builder.ToTable("stored_files");
        builder.Property(f => f.Id).HasColumnName("id");
        builder.HasKey(f => f.Id);
        builder.Property(f => f.UpdatedAt).HasColumnName("updated_at");
        builder.Property(f => f.CreatedAt).HasColumnName("created_at");
        builder.Property(f => f.Path).HasColumnName("path");
        builder.Property(f => f.Name).HasColumnName("name");
        builder.Property(f => f.MimeType).HasColumnName("mime_type");
    }    
}
