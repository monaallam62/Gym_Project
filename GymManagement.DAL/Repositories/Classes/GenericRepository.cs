using Gym_Project.Contexts;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        //Database Connection
        private readonly GymDbContext _dbContext;
        private readonly DbSet<TEntity> _set;
        public GenericRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
            _set = _dbContext.Set<TEntity>();
        }
        public async Task<int> AddAsync(TEntity entity)
        {
            _set.Add(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(TEntity entity)
        {
            _set.Remove(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
            IQueryable<TEntity> query=tracking ? _set : _set.AsNoTracking();
            return await query.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _set.FindAsync(id, ct);

        public async Task<int> UpdateAsync(TEntity entity)
        {
            _set.Update(entity);
            return await _dbContext.SaveChangesAsync();
        }
    }
}
