namespace NestNotification.API.ModelConfigurations;

public class EmailLogConfiguration : IEntityTypeConfiguration<EmailLog>
{
    public void Configure(EntityTypeBuilder<EmailLog> builder)
    {
        builder.Property(x => x.ToEmail)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(x => x.Body)
            .IsRequired(false)
            .HasMaxLength(10000);

        builder.Property(x => x.Subject)
            .IsRequired(false)
            .HasMaxLength(128);

        builder.Property(x => x.SentAt).IsRequired();
    }
}