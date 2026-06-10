using Gym_Project.Contexts;
using Gym_Project.Models;
using GymManagement.DAL.Repositories.Classes;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Gym_Project.Controllers
{ 
    public class PlanController : Controller
    {
        // [1] Database Connection
        //private readonly GymDbContext context;

        private readonly IGenericRepository<Plan> _planRepository;


        public PlanController(IGenericRepository<Plan> planRepository)
        {
            _planRepository = planRepository;
        }

        //Get :: BaseUrl/Plan/Index.
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var plans = await _planRepository.GetAllAsync(ct:ct); //pass by name to avoid ambiguity with the cancellation token parameter
            return View(plans);
        }
        //GET :: BaseUrl/Plan/Details/{id}
        public async Task<IActionResult> Details(int id ,CancellationToken ct)
        {
            var plan = await _planRepository.GetByIdAsync(id, ct);
            if (plan == null)
                return RedirectToAction(nameof(Index));
            return View(plan);
        }
    }
}
