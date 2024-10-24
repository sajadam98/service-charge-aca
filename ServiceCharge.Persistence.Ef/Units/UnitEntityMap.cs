using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ServiceCharge.Persistence.Ef.Units;

public class UnitEntityMap:IEntityTypeConfiguration<Unit>
{
    public void Configure(EntityTypeBuilder<Unit> builder)
    {
        builder.ToTable("Units");
        builder.HasKey(_ => _.Id);
        builder.Property(_ => _.Id).ValueGeneratedOnAdd();
        builder.Property(_ => _.Name).IsRequired();
        builder.Property(_ => _.FloorId).IsRequired();
        builder.Property(_ => _.IsActive).IsRequired();
    }
}