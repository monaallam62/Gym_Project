using Gym_Project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gym_Project.FluentConfiguration
{
    public class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.Property(p => p.Name)
                .HasColumnType("varchar(30)");
            builder.Property(p => p.Description)
                .HasMaxLength(200);
            builder.Property(p => p.Price)
                .HasPrecision(10, 2);
            builder.Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETDATE()");
            builder.ToTable(TB =>
            {
                TB.HasCheckConstraint("PlanDurationCheck", "DurationDays Between 1 and 365");
            });
        }
    }
}
