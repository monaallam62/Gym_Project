using GymMangement.BLL.Services.Interfaces;
using GymMangement.BLL.ViewModels.TrainerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangement.BLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        public Task<bool> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TrainerViewModel>> GetAllTrainerAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<TrainerViewModel> GetTrainerDetailsAsync(int trainerId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<TrainerToUpdateViewModel> GetTrainerToUpdateAsync(int trainerId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveTrainerAsync(int trainerId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateTrainerDetailsAsync(int trainerId, TrainerToUpdateViewModel model, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
