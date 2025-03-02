namespace NestNotification.API.ModelConfigurations;

public class FailedEmailConfigurations : IEntityTypeConfiguration<FailedEmail>
{
    public void Configure(EntityTypeBuilder<FailedEmail> builder)
    {
        builder.Property(x => x.To).IsRequired();
        builder.Property(x => x.Subject).IsRequired();
        builder.Property(x => x.Body).IsRequired();
        builder.Property(x => x.TemplateId).IsRequired(false);
        builder.Property(x => x.RetryCount).IsRequired();
        builder.Property(x => x.LastAttempt).IsRequired();
    }
}