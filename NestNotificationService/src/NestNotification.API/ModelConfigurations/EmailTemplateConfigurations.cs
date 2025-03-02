namespace NestNotification.API.ModelConfigurations;

public class EmailTemplateConfigurations : IEntityTypeConfiguration<EmailTemplate>
{
    public void Configure(EntityTypeBuilder<EmailTemplate> builder)
    {
        builder.Property(x => x.TemplateName).IsRequired();

        builder.Property(x => x.Subject).IsRequired(false);

        builder.Property(x => x.Body).IsRequired();

        builder.Property(x => x.CreatedAt).IsRequired();
    }
}