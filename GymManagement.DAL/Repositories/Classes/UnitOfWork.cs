using Gym_Project.Contexts;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        //DB Connection 
        private readonly GymDbContext _dbContext;
        private readonly Dictionary<string, object> _repsitories = [];
        public UnitOfWork(GymDbContext dbContext , ISessionRepository sessionRepository)
        {
            _dbContext = dbContext;
            SessionRepository =sessionRepository
        }

        public ISessionRepository SessionRepository { get; }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            //Check if Repo Exist Or Not ?
            //IGenricRepository<Member> >Name
            var TypeName = typeof(TEntity).Name;

            // If Exist In Dictionary => Use It
            if (_repsitories.TryGetValue(TypeName, out object? value))
                return (IGenericRepository<TEntity>)value;
            //If Not Exist
            //Create Repo => Add It To Dictionary => Return New Repo
            else
            {
                // IF Not -> Create -> Store -> Return 
                var repo = new GenericRepository<TEntity>(_dbContext);
                _repsitories[TypeName]=repo; //Store in Dictionary
                return repo;
            }

        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await _dbContext.SaveChangesAsync(ct);
    }
    
}
