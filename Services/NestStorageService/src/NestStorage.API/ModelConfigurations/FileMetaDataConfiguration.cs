namespace NestStorage.API.ModelConfigurations;

public class FileMetaDataConfiguration : IEntityTypeConfiguration<FileMetadata>
{
    public void Configure(EntityTypeBuilder<FileMetadata> builder)
    {
        builder.Property(x => x.Key)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.Value)
            .IsRequired()
            .HasMaxLength(255);
    }
}