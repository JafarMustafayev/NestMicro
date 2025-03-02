namespace NestNotification.API.ModelConfigurations;

public class EmailQueueConfigurations : IEntityTypeConfiguration<EmailQueue>
{
    public void Configure(EntityTypeBuilder<EmailQueue> builder)
    {
        builder.Property(x => x.ToEmail).IsRequired();

        builder.Property(x => x.Subject).IsRequired();

        builder.Property(x => x.Body).IsRequired();

        builder.Property(x => x.RetryCount).IsRequired().HasDefaultValue(0);

        builder.Property(x => x.ErrorMessage).IsRequired(false);

        builder.Property(x => x.LastAttempt).IsRequired();
    }
}