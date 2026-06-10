using GymMangement.BLL.Services.Interfaces;
using GymMangement.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Gym_Project.PL.Controllers
{
    public class MembersController : Controller
    {
        //MemberService
        public readonly IMemberService _memService;

        public MembersController(IMemberService memService)
        {
            _memService = memService;
        }

        #region Get Members
        //Get :: BaseUrl/(Member)ControllerName/Index =>List all member in DB
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            //Call Service => GetAllMembers
            var members =await _memService.GetAllAsync(ct);
            return View(members);
        }

        //Get :: BaseUrl/Members/Details/{id} => Get Specific Member
        //Get :: BaseUrl/Members/HealthRecordDetails/{id} => Get Data of Specific Member With HealthRecord

        #endregion
        #region Create
        //Get :: BaseUrl/Members/Create =>Show Empty Form
        [HttpGet]
        public IActionResult Create() 
            => View();
        //Post:: BaseUrl/Member/Create/{Member} =>Submit Form
        //CreateMember
        [HttpPost]
        public async Task<IActionResult> CreateMember(CreateMemberViewModel model ,CancellationToken ct)
        {
            //Check ModelState
            if (!ModelState.IsValid) return View(nameof(Create), model);

            var result = await _memService.CreateMemberAsync(model, ct);
            return RedirectToAction(nameof(Index), result);
        }
        
        #endregion
        #region Edit
        //Get :: BaseUrl/Members/Edit/{id} => Show Edit Form
        //Post BaseUrl/Members/Edit/{member} => Submit Edit Form
        #endregion
        #region Delete
        //Get :: BaseUrl/Member/Delete/{id} => Show Validation Padge
        #endregion
    }
}
