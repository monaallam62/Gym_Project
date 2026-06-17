using GymMangement.BLL.ViewModels.PlanViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangement.BLL.Services.Interfaces
{
    public interface IPlanService
    {
        Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct=default);
        Task<PlanViewModel?> GetPlanByIdAsync(int planid, CancellationToken ct=default); 
        Task<UpdatePlanViewModel?> GetPlanToUpdateAsync(int palnId, CancellationToken ct = default); // Edited ?
        Task<bool> ToggleActivationAsync(int planId, CancellationToken ct=default);
        Task<bool> UpdatePlanAsync(int id ,UpdatePlanViewModel model, CancellationToken ct=default);
    }
}
