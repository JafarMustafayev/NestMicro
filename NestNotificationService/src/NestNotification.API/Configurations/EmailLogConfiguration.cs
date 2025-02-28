namespace NestNotification.API.Configurations;

public class EmailLogConfiguration : IEntityTypeConfiguration<EmailLog>
{
    public void Configure(EntityTypeBuilder<EmailLog> builder)
    {
        builder.Property(x => x.To).IsRequired();
        builder.Property(x => x.Subject).IsRequired();
        builder.Property(x => x.Body).IsRequired();
        builder.Property(x => x.TemplateId).IsRequired(false);
        builder.Property(x => x.SentDate).IsRequired();
        builder.Property(x => x.WhoCreated).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.IsDeleted).IsRequired();
        builder.Property(x => x.IsActive).IsRequired();
    }
}