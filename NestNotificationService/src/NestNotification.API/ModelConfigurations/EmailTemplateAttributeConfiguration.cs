namespace NestNotification.API.ModelConfigurations;

public class EmailTemplateAttributeConfiguration:IEntityTypeConfiguration<EmailTemplateAttribute>
{
    public void Configure(EntityTypeBuilder<EmailTemplateAttribute> builder)
    {
        builder.Property(x => x.AttributeName)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(x => x.AttributeValue)
            .IsRequired()
            .HasMaxLength(128);
        
        builder.Property(x => x.Description)
            .IsRequired(false)
            .HasMaxLength(10000);
    }
}