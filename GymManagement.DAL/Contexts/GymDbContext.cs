using Gym_Project.FluentConfiguration;
using Gym_Project.Models;
using GymManagement.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Gym_Project.Contexts
{
    public class GymDbContext:DbContext
    {

        public GymDbContext(DbContextOptions<GymDbContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Plan> plans { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }

    }
}
