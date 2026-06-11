using GymManagement.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.FluentConfiguration
{
    public class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(X => X.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50);
            builder.Property(X => X.Email)
                .HasColumnType("varchar")
                .HasMaxLength(100);
            builder.HasIndex(X => X.Email).IsUnique();
            builder.HasIndex(X => X.Phone).IsUnique();
            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("EmailCheck","Email LIKE '_%@_%._%'");
                tb.HasCheckConstraint("PhoneCheck", "Phone LIKE '010%' or  Phone LIKE '011%' or  Phone LIKE '012%' or Phone LIKE '015%'");
            });
            //Address Owned Entity Type 
            builder.OwnsOne(X => X.Address, address =>
                {
                    address.Property(a => a.City).HasColumnType("City")
                    .HasColumnType("varchar")
                    .HasMaxLength(30);
                    address.Property(a => a.Street).HasColumnType("Street")
                    .HasColumnType("varchar")
                    .HasMaxLength(30);

                });
        }
    }
}
