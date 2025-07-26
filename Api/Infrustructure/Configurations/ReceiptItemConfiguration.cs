using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrustructure.Configurations;

public class ReceiptItemConfiguration : IEntityTypeConfiguration<ReceiptItem> {
    public void Configure(EntityTypeBuilder<ReceiptItem> builder) {
        builder.ToTable("receipt_items");
        builder.Property(f => f.Id).HasColumnName("id");
        builder.HasKey(f => f.Id);
        builder.Property(f => f.UpdatedAt).HasColumnName("updated_at");
        builder.Property(f => f.CreatedAt).HasColumnName("created_at");
        builder.Property(f => f.ReceiptId).HasColumnName("receipt_id");
        builder.Ignore(f => f.Receipt);

        builder.Property(f => f.ProductName).HasColumnName("product_name");
        builder.Property(f => f.Quantity).HasColumnName("quantity");
        builder.Property(f => f.Unit).HasColumnName("unit");
        builder.Property(f => f.PricePerUnit).HasColumnName("price_per_unit");
        builder.Property(f => f.TotalPrice).HasColumnName("total_price");

        builder.HasOne<Receipt>()
            .WithMany(r => r.Items)
            .HasForeignKey(r => r.ReceiptId)
            .HasConstraintName("FK_receipt_items_receipts_receipt_id");
    }    
}
