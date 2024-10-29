using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceCharge.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCharge.Persistence.Ef.Units
{
    public class UnitEntitymap : IEntityTypeConfiguration<Unit>
    {
        public void Configure(EntityTypeBuilder<Unit> builder)
        {
            builder.ToTable("Units");

            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Id)
                .ValueGeneratedOnAdd();
            builder
                .Property(_ => _.Name)
                .IsRequired();
            builder
            .Property(_ => _.IsActive)
                .IsRequired();



            builder.Property(_ => _.FloorId)
               .IsRequired();

        }
    }
}
