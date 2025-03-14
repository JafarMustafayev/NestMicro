namespace NestNotification.API.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<EmailTemplate> EmailTemplates { get; set; }
    public DbSet<EmailLog> EmailLogs { get; set; }
    public DbSet<EmailQueue> EmailQueues { get; set; }
    public DbSet<EmailTemplateAttribute> EmailTemplateAttributes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EmailLogConfiguration).Assembly);
    }
}