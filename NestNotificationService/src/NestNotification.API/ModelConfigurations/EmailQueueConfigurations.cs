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

        builder.Property(x => x.CreatedAt).IsRequired();

        builder.Property(x => x.Priority).IsRequired().HasDefaultValue(EmailPriority.Normal);

        builder.Property(x => x.Status).IsRequired().HasDefaultValue(EmailStatus.Pending);
    }
}