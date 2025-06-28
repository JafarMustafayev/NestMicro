namespace NestStorage.API.ModelConfigurations;

public class StorageBucketConfiguration : IEntityTypeConfiguration<StorageBucket>
{
    public void Configure(EntityTypeBuilder<StorageBucket> builder)
    {
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(x => x.Provider).IsRequired();

        builder.Property(x => x.BasePath)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(x => x.IsPublic)
            .IsRequired()
            .HasDefaultValue(true);
    }
}