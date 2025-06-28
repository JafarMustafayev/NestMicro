namespace NestStorage.API.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    private DbSet<Entities.File> Files { get; set; }
    private DbSet<FileMetaData> FileMetaData { get; set; }
    private DbSet<StorageBucket> StorageBuckets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FileConfiguration).Assembly);
    }
}