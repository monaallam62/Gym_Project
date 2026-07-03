using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity :BaseEntity , new()
    {
        //GetById
        Task<TEntity?> GetByIdAsync(int id, CancellationToken ct=default);
        //Add
        void Add(TEntity entity);
        //Update
        void Update(TEntity entity);
        //Delete
        void Delete(TEntity entity);
        //GetAll
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null, bool tracking=false ,CancellationToken ct=default);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate ,CancellationToken ct =default);
        Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken ct = default);
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,bool tracking = false ,CancellationToken ct = default);
    }
}
