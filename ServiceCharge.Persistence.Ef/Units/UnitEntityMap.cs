using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceCharge.Entities;

namespace ServiceCharge.Persistence.Ef.Units;

public class UnitEntityMap:IEntityTypeConfiguration<Unit>
{
    public void Configure(EntityTypeBuilder<Unit> builder)
    {
        builder.ToTable("Units");
        builder.HasKey("Id");
        builder.Property(_ => _.Id).ValueGeneratedOnAdd();
        builder.Property(_ => _.Name).IsRequired().HasMaxLength(100);
        builder.Property(_ => _.FloorId).IsRequired();
        builder.Property(_ => _.IsActive).IsRequired();
    }
}