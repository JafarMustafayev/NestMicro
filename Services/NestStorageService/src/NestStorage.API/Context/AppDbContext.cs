namespace NestStorage.API.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Entities.File> Files { get; set; }
    public DbSet<StorageBucket> StorageBuckets { get; set; }
    public DbSet<FileMetaData> FileMetaData { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FileConfiguration).Assembly);
    }
}