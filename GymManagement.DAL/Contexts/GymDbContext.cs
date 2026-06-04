using Gym_Project.FluentConfiguration;
using Gym_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Gym_Project.Contexts
{
    public class GymDbContext:DbContext
    {

        public GymDbContext(DbContextOptions<GymDbContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<Plan>(new PlanConfiguration());
        }

        public DbSet<Plan> plans { get; set; }
        
    }
}
