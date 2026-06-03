using Gym_Project.Contexts;
using Gym_Project.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Classes
{
    public class PlanRepository : IPlanRepository
    {
        //[1]DATABASE CONNECTION
        private readonly GymDbContext dbcontext;
        public PlanRepository()
        {
            dbcontext = new GymDbContext();
        }
        public async Task<int> AddAsync(Plan plan, CancellationToken ct = default)
        {
            dbcontext.plans.Add(plan);
            return await dbcontext.SaveChangesAsync(ct);
        }

        public async Task<int> DeleteAsync(Plan plan, CancellationToken ct = default)
        {
            dbcontext.plans.Remove(plan);
            return await dbcontext.SaveChangesAsync(ct);

        }

        public async Task<IEnumerable<Plan>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
            IQueryable<Plan> query = tracking ? dbcontext.plans : dbcontext.plans.AsNoTracking();
            return await query.ToListAsync(ct);
        }

        public async Task<Plan?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await dbcontext.plans.FindAsync(id,ct);
        }

        public Task UpdateAsync(Plan plan, CancellationToken ct = default)
        {
            dbcontext.plans.Update(plan);
            return dbcontext.SaveChangesAsync(ct);
        }
    }
}
