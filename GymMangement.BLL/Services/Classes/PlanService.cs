using Gym_Project.Models;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using GymMangement.BLL.Services.Interfaces;
using GymMangement.BLL.ViewModels.PlanViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangement.BLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        //DataConnection 
        private readonly IGenericRepository<Plan> _planRepository;
        private readonly IGenericRepository<Membership> _membershipRepository;

        public PlanService(IGenericRepository<Plan> planRepository, IGenericRepository<Membership> membershipRepository)
        {
            _planRepository = planRepository;
            _membershipRepository = membershipRepository;
        }

        public async Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default)
        {
            var plans = await _planRepository.GetAllAsync(ct: ct);
            return plans.Select(p => new PlanViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                DurationDays = p.DurationDays,
                Description = p.Description,
                IsActive = p.IsActive
            });
        }

        public async Task<PlanViewModel?> GetPlanByIdAsync(int planid, CancellationToken ct = default)
        {
            var plan = await _planRepository.GetByIdAsync(planid, ct);
            if (plan is null) return null;
            else
                return new PlanViewModel
                {
                    Name = plan.Name,
                    Price = plan.Price,
                    DurationDays = plan.DurationDays,
                    Description = plan.Description,
                    IsActive = plan.IsActive
                };
        }

        public async Task<UpdatePlanViewModel> GetPlanToUpdateAsync(int planId, CancellationToken ct = default)
        {
            var plan = await _planRepository.GetByIdAsync(planId, ct);
            if (plan is null || !plan.IsActive) return null;
            if (await HasActiveMembershipsAsync(planId, ct))
                return null;
            else
                return new UpdatePlanViewModel
                {
                    PlanName = plan.Name,
                    Price = plan.Price,
                    DurationDays = plan.DurationDays,
                    Description = plan.Description
                };
        }

        public async Task<bool> ToggleActivationAsync(int planId, CancellationToken ct = default)
        {
            var plan = await _planRepository.GetByIdAsync(planId, ct);
            if (plan is null) return false;
            if (plan.IsActive && await HasActiveMembershipsAsync(planId, ct))
                return false;
            plan.IsActive = !plan.IsActive;
            plan.UpdatedAt = DateTime.Now;
            var result = await _planRepository.UpdateAsync(plan, ct);
            return result > 0;
        }

        public async Task<bool> UpdatePlanAsync(int id, UpdatePlanViewModel model, CancellationToken ct = default)
        {
            var plan =await _planRepository.GetByIdAsync(id, ct);
            if(plan is null) return false;
            if(await HasActiveMembershipsAsync(id,ct))
                return false;

            plan.DurationDays = model.DurationDays;
            plan.Description = model.Description;
            plan.Price = model.Price;
            plan.UpdatedAt = DateTime.Now; 
             
            var result = await _planRepository.UpdateAsync(plan, ct);
            return result > 0;
        }

        private async Task<bool> HasActiveMembershipsAsync(int planId, CancellationToken ct)
        {
            return await _membershipRepository.AnyAsync(m => m.PlanId == planId && m.EndDate > DateTime.Now, ct);

        }
    }
}
