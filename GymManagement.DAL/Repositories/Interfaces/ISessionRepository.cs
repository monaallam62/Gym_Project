using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Interfaces
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        Task<IEnumerable<Session>> GetAllSessionswithTrainerAndCategory(CancellationToken ct = default);
        Task<int> GetCountOfBookedSlotAsync(int sessionId ,CancellationToken ct =default);
        Task<Session?> GetSessionByIdWithTrainAndCategory(int sessionId ,CancellationToken ct =default);
    }
}
