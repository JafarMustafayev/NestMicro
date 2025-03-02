namespace NestNotification.API.ModelConfigurations;

public class EmailLogConfiguration : IEntityTypeConfiguration<EmailLog>
{
    public void Configure(EntityTypeBuilder<EmailLog> builder)
    {
        builder.Property(x => x.ToEmail).IsRequired();

        builder.Property(x => x.Body).IsRequired(false);

        builder.Property(x => x.Subject).IsRequired(false);

        builder.Property(x => x.SentAt).IsRequired();
    }
}