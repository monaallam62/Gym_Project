using GymManagement.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.FluentConfiguration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Category> builder)
        {
            builder.Property(X => X.CategoryName)
                .HasColumnType("varchar")
                .HasMaxLength(30);

            builder.Property(X => X.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            //Seeding =>>> HasData =>Must send ID ,Disable Identity ID
            builder.HasData(
                new Category { Id = 1, CategoryName = "Cardio" },
                new Category { Id = 2, CategoryName = "Strength Training" },
                new Category { Id = 3, CategoryName = "Flexibility" },
                new Category { Id = 4, CategoryName = "Balance" }
            );
        }
    }
}
