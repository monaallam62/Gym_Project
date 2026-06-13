using Gym_Project.Contexts;
using Gym_Project.Models;
using GymManagement.DAL.Repositories.Classes;
using GymManagement.DAL.Repositories.Interfaces;
using GymMangement.BLL.Services.Interfaces;
using GymMangement.BLL.ViewModels.PlanViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Gym_Project.Controllers
{
    public class PlanController : Controller
    {
        // [1] Database Connection
        //private readonly GymDbContext context;
        private readonly IPlanService _planService;

        //private readonly IGenericRepository<Plan> _planRepository;


        public PlanController(IGenericRepository<Plan> planRepository, IPlanService planService)
        {
            //_planRepository = planRepository;
            _planService = planService;
        }

        //Get :: BaseUrl/Plan/Index.
        public async Task<IActionResult> Index(CancellationToken ct)
            => View(await _planService.GetAllPlansAsync(ct));
        //public async Task<IActionResult> Index(CancellationToken ct)
        //{
        //    var plans = await _planRepository.GetAllAsync(ct:ct); //pass by name to avoid ambiguity with the cancellation token parameter
        //    return View(plans);
        //}
        //GET :: BaseUrl/Plan/Details/{id}
        //public async Task<IActionResult> Details(int id ,CancellationToken ct)
        //{
        //    var plan = await _planRepository.GetByIdAsync(id, ct);
        //    if (plan == null)
        //        return RedirectToAction(nameof(Index));
        //    return View(plan);
        //}
        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var plan = await _planService.GetPlanByIdAsync(id, ct);
            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        { 
            var plan = await _planService.GetPlanToUpdateAsync(id, ct);
            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan cannot be edited (Not Found, InActive or Has Active Membership).";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdatePlanViewModel model , CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _planService.UpdatePlanAsync(id, model, ct); 
            if (result)
                TempData["SuccessMessage"] = "Plan updated successfully.";
            else
                TempData["ErrorMessage"] = "Plan Failed to update.";
                return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Activate(int id , CancellationToken ct)
            {
            var result = await _planService.ToggleActivationAsync(id, ct);
            if (result)
                TempData["SuccessMessage"] = "Plan Status Changed successfully.";
            else
                TempData["ErrorMessage"] = "Failed to Toggle Plan Status.";
            return RedirectToAction(nameof(Index));
        }
    }
}
