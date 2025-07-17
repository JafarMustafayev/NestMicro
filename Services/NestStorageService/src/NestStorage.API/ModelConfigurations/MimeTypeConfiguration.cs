namespace NestStorage.API.ModelConfigurations;

public class MimeTypeConfiguration : IEntityTypeConfiguration<MimeType>
{
    public void Configure(EntityTypeBuilder<MimeType> builder)
    {
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(x => x.Extension)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(x => x.MaxUploadSizeInBytes)
            .IsRequired()
            .HasDefaultValue(0);
    }
}