using GymManagement.DAL.Models;
using GymMangement.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gym_Project.PL.Controllers
{
    public class SessionsController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionsController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var Sessions = await _sessionService.GetAllSessionsAsync(ct);
            return View(Sessions);
        }
    }
}
