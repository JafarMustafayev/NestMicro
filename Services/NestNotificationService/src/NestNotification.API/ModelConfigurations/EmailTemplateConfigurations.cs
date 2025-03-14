namespace NestNotification.API.ModelConfigurations;

public class EmailTemplateConfigurations : IEntityTypeConfiguration<EmailTemplate>
{
    public void Configure(EntityTypeBuilder<EmailTemplate> builder)
    {
        builder.Property(x => x.TemplateName)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(x => x.Subject)
            .IsRequired(false)
            .HasMaxLength(128);

        builder.Property(x => x.Body)
            .IsRequired()
            .HasMaxLength(10000);

        builder.Property(x => x.CreatedAt).IsRequired();
    }
}