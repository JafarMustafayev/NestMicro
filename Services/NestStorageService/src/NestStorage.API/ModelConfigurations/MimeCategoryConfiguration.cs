namespace NestStorage.API.ModelConfigurations;

public class MimeCategoryConfiguration : IEntityTypeConfiguration<MimeCategory>
{
    public void Configure(EntityTypeBuilder<MimeCategory> builder)
    {
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(128);
    }
}