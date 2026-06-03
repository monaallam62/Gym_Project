using Gym_Project.FluentConfiguration;
using Gym_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Gym_Project.Contexts
{
    public class GymDbContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQL2026;Database=GymDb;Trusted_Connection=True;trustServerCertificate=True"); 
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<Plan>(new PlanConfiguration());
        }

        public DbSet<Plan> plans { get; set; }
    }
}
