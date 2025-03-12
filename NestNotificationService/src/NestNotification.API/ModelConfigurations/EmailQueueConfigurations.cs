namespace NestNotification.API.ModelConfigurations;

public class EmailQueueConfigurations : IEntityTypeConfiguration<EmailQueue>
{
    public void Configure(EntityTypeBuilder<EmailQueue> builder)
    {
        builder.Property(x => x.ToEmail)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(x => x.Subject)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(x => x.Body)
            .IsRequired()
            .HasMaxLength(10000);

        builder.Property(x => x.RetryCount)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(x => x.ErrorMessage)
            .IsRequired(false)
            .HasMaxLength(10000);

        builder.Property(x => x.LastAttempt).IsRequired();

        builder.Property(x => x.CreatedAt).IsRequired();

        builder.Property(x => x.Priority).IsRequired();

        builder.Property(x => x.Status).IsRequired();
    }
}