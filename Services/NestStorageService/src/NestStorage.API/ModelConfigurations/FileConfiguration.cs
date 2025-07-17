namespace NestStorage.API.ModelConfigurations;

public class FileConfiguration : IEntityTypeConfiguration<Entities.File>
{
    public void Configure(EntityTypeBuilder<Entities.File> builder)
    {
        builder.Property(x => x.FileName)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.StorageName)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.RelativePath)
            .HasMaxLength(500)
            .IsRequired();

        builder.Ignore(x => x.FullPath);

        builder.Property(x => x.Size)
            .IsRequired();

        builder.Property(x => x.RelatedEntityId)
            .HasMaxLength(36);

        builder.Property(x => x.RelatedEntityType)
            .HasMaxLength(50);

        builder.Property(x => x.FromIp)
            .HasMaxLength(15)
            .IsRequired()
            .HasDefaultValue("0.0.0.0");

        builder.Property(x => x.IsPublic)
            .HasDefaultValue(true);
    }
}