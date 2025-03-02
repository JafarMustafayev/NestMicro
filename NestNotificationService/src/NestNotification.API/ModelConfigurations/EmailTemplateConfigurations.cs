namespace NestNotification.API.ModelConfigurations;

public class EmailTemplateConfigurations : IEntityTypeConfiguration<EmailTemplate>
{
    public void Configure(EntityTypeBuilder<EmailTemplate> builder)
    {
        builder.Property(x => x.TemplateName).IsRequired();
        builder.Property(x => x.Subject).IsRequired();
        builder.Property(x => x.HtmlBody).IsRequired();
    }
}