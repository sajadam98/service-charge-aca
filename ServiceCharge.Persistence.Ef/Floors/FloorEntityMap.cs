using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceCharge.Entities;

namespace ServiceCharge.Persistence.Ef.Floors;

public class FloorEntityMap : IEntityTypeConfiguration<Floor>
{
    public void Configure(EntityTypeBuilder<Floor> builder)
    {
        builder.ToTable("Floors");
        builder.HasKey(_ => _.Id);
        builder.Property(_ => _.Id).UseIdentityColumn();
        builder.Property(_ => _.BlockId).IsRequired();
        builder.Property(_ => _.Name).IsRequired();
        builder.Property(_ => _.UnitCount).IsRequired();
    }
}