namespace NestStorage.API.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Entities.File> Files { get; set; }
    public DbSet<StorageBucket> StorageBuckets { get; set; }
    public DbSet<FileMetadata> FileMetaData { get; set; }
    public DbSet<MimeCategory> MimeCategories { get; set; }
    private DbSet<MimeType> MimeTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FileConfiguration).Assembly);
    }
}