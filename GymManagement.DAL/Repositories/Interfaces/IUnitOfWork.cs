using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        public IMembershipRepository MembershipRepository { get; }
        public IBookingRepository BookingRepository { get; } 
        //Get Repository
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new();
        //SaveChanges
        Task<int> SaveChangesAsync(CancellationToken ct = default); // or Complete Name instead of SaveChangesAsync
         public ISessionRepository SessionRepository { get; }
    }
}
