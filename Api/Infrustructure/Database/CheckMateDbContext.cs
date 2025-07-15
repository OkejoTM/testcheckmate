namespace Infrustructure.Database;

using Domain.Entities;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

public class CheckMateDbContext : DbContext {
    public static readonly string ConnectionFieldName = "PostgresConnection";
    public DbSet<Receipt> Receipts { set; get; }

    public DbSet<StoredFile> StoredFiles { set; get; }
    public CheckMateDbContext(DbContextOptions<CheckMateDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
