using GymMangement.BLL.Services.Interfaces;
using GymMangement.BLL.ViewModels;
using GymMangement.BLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Gym_Project.PL.Controllers
{
    public class MembersController : Controller
    {
        //MemberService
        public readonly IMemberService _memService;
        private readonly IAttachmentService _attachService;

        public MembersController(IMemberService memService , IAttachmentService attachService)
        {
            _memService = memService;
            _attachService = attachService;
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
        public async Task<IActionResult> MemberDetails(int id, CancellationToken ct)
        {
            var member =await _memService.GetMemberDetailsByIdAsync(id, ct);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found!";
            }
            return View(member);
        }
        //Get :: BaseUrl/Members/HealthRecordDetails/{id} => Get Data of Specific Member With HealthRecord
        public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken ct)
        {
            var record = await _memService.GetMemberHealthRecord(id, ct);
            if (record is null)
            {
                TempData["ErrorMessage"] = "Health Record Not Found!";
                return RedirectToAction(nameof(Index));
            }
            return View(record);
        }

        //Action To Get MemberPhoto
        [HttpGet]
        public async Task<IActionResult> Picture(int id)
        {
            var member = await _memService.GetMemberDetailsByIdAsync(id);
            if(member is null || string.IsNullOrWhiteSpace(member.Photo))
                return NotFound();
            var result = _attachService.GetFile(member.Photo, "MembersPhoto");
            if(result is null) return NotFound();
            return File(result.Value.stream, result.Value.contentType);
        }
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
            if (result)
                TempData["SuccessMessage"] = "Member Created Successfully";
            else
                TempData["ErrorMessage"] = "Member is Already Exist or Failed TO Create!";
                return RedirectToAction(nameof(Index), result);
        }

        #endregion
        #region Edit
        //Get :: BaseUrl/Members/Edit/{id} => Show Edit Form
        [HttpGet]
        public async Task<IActionResult> EditMember(int id, CancellationToken ct)
        {
            var member = await _memService.GetMemberToUpdateAsync(id, ct);
            if (member == null)
            {
                TempData["ErrorMessage"] = "Member Not Found!";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        //Post BaseUrl/Members/Edit/{member} => Submit Edit Form
        [HttpPost]
        public async Task<IActionResult> EditMember([FromRoute]int id, MemberToUpdateViewModel model, CancellationToken ct)
        {
            //Check Model State   
            if (!ModelState.IsValid) return View(model);

            var result = await _memService.UpdateMemberAsync(id, model, ct);
            if (result)
                TempData["SuccessMessage"] = "Member Updated Successfully";
            else
                TempData["ErrorMessage"] = "Failed to Update Member!";
            return RedirectToAction(nameof(Index));
        }
        #endregion
        #region Delete
        //Get :: BaseUrl/Member/Delete/{id} => Show Validation Padge
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        { 
            var member = await _memService.GetMemberDetailsByIdAsync(id, ct);

            if (member is null)
            { 
                TempData["ErrorMessage"] = "Member Not Found!";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed([FromRoute]int id, CancellationToken ct)
        {
            var result = await _memService.DeleteMemberAsync(id, ct);
            if (result)
                TempData["SuccessMessage"] = "Member Deleted Successfully";
            else
                TempData["ErrorMessage"] = "Failed to Delete Member! Maybe Member has Active Booking";
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
