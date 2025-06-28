namespace NestStorage.API.ModelConfigurations;

public class FileMetaDataConfiguration : IEntityTypeConfiguration<FileMetaData>
{
    public void Configure(EntityTypeBuilder<FileMetaData> builder)
    {
        builder.Property(x => x.Key)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.Value)
            .IsRequired()
            .HasMaxLength(255);
    }
}