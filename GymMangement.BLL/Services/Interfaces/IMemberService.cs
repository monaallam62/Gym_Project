using GymManagement.DAL.Models;
using GymMangement.BLL.ViewModels;
using GymMangement.BLL.ViewModels.MemberViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangement.BLL.Services.Interfaces
{
    public interface IMemberService
    {
        //Gett All
        Task<IEnumerable<MemberViewModel>> GetAllAsync(CancellationToken ct = default);
        //Create Member
        Task<bool> CreateMemberAsync(CreateMemberViewModel member, CancellationToken ct = default);
        Task<MemberViewModel> GetMemberDetailsByIdAsync(int memberId, CancellationToken ct = default);
        //Get Member HelthRecord
        Task<HealthRecordViewModel> GetMemberHealthRecord (int memberId, CancellationToken ct);
        //Get member To Update
        Task<MemberToUpdateViewModel> GetMemberToUpdateAsync(int memberId , CancellationToken ct = default);
        //Update Member
        Task<bool> UpdateMemberAsync(int Id, MemberToUpdateViewModel model, CancellationToken ct = default);

        //Delete Member
         Task<bool> DeleteMemberAsync(int memberId, CancellationToken ct = default);
    }
}
