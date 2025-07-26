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

        builder.Property(f => f.UploadedByUserId).HasColumnName("uploaded_by_user_id");
        builder.Property(f => f.OperationType).HasColumnName("operation_type")
            .HasConversion<string>()
            .HasMaxLength(50);
        builder.Property(f => f.CategoryByStore).HasColumnName("category_by_store")
            .HasConversion<string>()
            .HasMaxLength(50);
        builder.Property(f => f.CategoryByPrice).HasColumnName("category_by_price")
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(f => f.Date).HasColumnName("date");
        builder.Property(f => f.Time).HasColumnName("time");
        builder.Property(f => f.Total).HasColumnName("total");
        builder.Property(f => f.FiscalNumber).HasColumnName("fiscal_number");
        builder.Property(f => f.FiscalDocument).HasColumnName("fiscal_document");
        builder.Property(f => f.FiscalSign).HasColumnName("fiscal_sign");
        builder.Property(f => f.INN).HasColumnName("inn");
        builder.Property(f => f.ReceiptNumber).HasColumnName("receipt_number");
        builder.Property(f => f.StoreName).HasColumnName("store_name");
        builder.Property(f => f.VatAmount).HasColumnName("vat_amount");

        builder.HasOne<StoredFile>()
            .WithMany()
            .HasForeignKey(r => r.FileId)
            .HasConstraintName("FK_receipts_stored_files_file_id");
    }    
}
