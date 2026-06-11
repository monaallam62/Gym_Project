using Gym_Project.Models;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using GymMangement.BLL.Services.Interfaces;
using GymMangement.BLL.ViewModels;
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
        private readonly IGenericRepository<Membership> _membershipRepo;
        private readonly IGenericRepository<Plan> _planRepo;
        private readonly IGenericRepository<HealthRecord> _healthRecordRepo;

        public MemberService(IGenericRepository<Member> memberRepo , IGenericRepository<Membership> membershipRepo , IGenericRepository<Plan> planRepo , IGenericRepository<HealthRecord> HealthRecordRepo)
        {
            _memberRepo = memberRepo;
            _membershipRepo = membershipRepo;
            _planRepo = planRepo;
            _healthRecordRepo = HealthRecordRepo;
        }

        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {
            //Email Exist or Not
            var EmailExist =await _memberRepo.AnyAsync(X => X.Email == model.Email);
            //Phone Exist Or Not
            var PhoneExist = await _memberRepo.AnyAsync(X => X.Phone == model.Phone);

            if (EmailExist || PhoneExist) return false;
            //Add Member
            var member = new Member()
            {
                Name =model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                Address = new Address
                {
                    BuildingNumber = model.BuildingNumber,
                    City = model.City,
                    Street = model.Street
                },
                HealthRecord =new HealthRecord()
                {
                    BloodType =model.HealthRecordViewModel.BloodType,
                    Height = model.HealthRecordViewModel.Height,
                    Weight = model.HealthRecordViewModel.Weight,
                    Note =model.HealthRecordViewModel.Note
                }
            };
            var result = await _memberRepo.AddAsync(member);
            return result > 0;
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

 

        public async Task<MemberViewModel> GetMemberDetailsByIdAsync(int memberId, CancellationToken ct = default)
        {
            //Get Mmeber By Id
            var member = await _memberRepo.GetByIdAsync(memberId, ct);
            if (member == null) return null;
            //Table = Member
            //Return =MemberViewModel
            var model = new MemberViewModel()
            {
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Photo = member.Photo,
                Gender = member.Gender.ToString(),
                DateOfBirth = member.DateOfBirth.ToShortDateString(),
                Address = $"{member.Address.BuildingNumber} {member.Address.Street} {member.Address.City}",
            };

            //check if the member has a ActiveMembership or not
            var ActiveMembership = await _membershipRepo.FirstOrDefaultAsync(X => X.MemberId == memberId && X.EndDate > DateTime.Now  );
            if (ActiveMembership is not null)
            {
                var ActivePlan = await _planRepo.GetByIdAsync(ActiveMembership.PlanId, ct);
                model.PlanName = ActivePlan.Name;
                model.MembershipStartDate = ActiveMembership.CreatedAt.ToString();
                model.MembershipEndDate = ActiveMembership.EndDate.ToString();
            }
            return model;
        }
        public async Task<HealthRecordViewModel> GetMemberHealthRecord(int memberId, CancellationToken ct)
        {
            var record = await _healthRecordRepo.FirstOrDefaultAsync(X => X.MemberId == memberId, ct: ct);

            if (record is null) return null;
            else
                return new HealthRecordViewModel
                {
                    Weight = record.Weight,
                    Height = record.Height,
                    BloodType = record.BloodType,
                    Note = record.Note
                };
        }
    }
}
