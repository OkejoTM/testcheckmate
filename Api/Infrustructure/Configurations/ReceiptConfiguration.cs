using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrustructure.Configurations;

public class ReceiptConfiguration : IEntityTypeConfiguration<Receipt> {
    public void Configure(EntityTypeBuilder<Receipt> builder) {
        builder.ToTable("receipts");
        builder.Property(f => f.Id).HasColumnName("id");
        builder.HasKey(f => f.Id);
        builder.Property(f => f.UpdatedAt).HasColumnName("updated_at");
        builder.Property(f => f.CreatedAt).HasColumnName("created_at");
        builder.Property(f => f.FileId).HasColumnName("file_id");
        builder.Property(f => f.Comment).HasColumnName("comment");
        builder.Property(f => f.State).HasColumnName("processing_state")
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.HasOne<Receipt>()
            .WithMany()
            .HasForeignKey(r => r.FileId)
            .HasConstraintName("FK_receipts_stored_files_file_id");
    }    
}
