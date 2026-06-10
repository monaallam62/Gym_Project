using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using GymMangement.BLL.Services.Interfaces;
using GymMangement.BLL.ViewModels.MemberViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangement.BLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        //Database Connection
        public readonly IGenericRepository<Member> _memberRepo;

        public MemberService(IGenericRepository<Member> memberRepo)
        {
            _memberRepo = memberRepo;
        }
        public async Task<IEnumerable<MemberViewModel>> GetAllAsync(CancellationToken ct = default)
        {

            //members come from Database
            var members = await _memberRepo.GetAllAsync(ct:ct);
            if (!members.Any()) return [];
            //Member => ViewModel

            List<MemberViewModel> memberVM = new List<MemberViewModel>();
            foreach (var member in members) 
            {
                //Data Comes From Database i need to send it to ViewModel
                //Manual Mapping
                var memberViewModel = new MemberViewModel()
                {
                    Name = member.Name,
                    Phone = member.Phone,
                    Photo= member.Photo,
                    Email =member.Email,
                    Id = member.Id,
                    Gender = member.Gender.ToString()
                };
                memberVM.Add(memberViewModel);
            }
            return memberVM;
        }
    }
}
