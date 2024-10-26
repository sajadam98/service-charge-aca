namespace ServiceCharge.Persistence.Ef.Blocks;

public class BlockEntityMap : IEntityTypeConfiguration<Block>
{
    public void Configure(EntityTypeBuilder<Block> builder)
    {
        builder.ToTable("Blocks");

        builder.HasKey(_ => _.Id);

        builder.Property(_ => _.Id).ValueGeneratedOnAdd();
    }
}