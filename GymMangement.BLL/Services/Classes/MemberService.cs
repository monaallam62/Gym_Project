using AutoMapper;
using Gym_Project.Models;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using GymMangement.BLL.Services.Interfaces;
using GymMangement.BLL.ViewModels;
using GymMangement.BLL.ViewModels.MemberViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangement.BLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAttachmentService _attachmentService;

        //UnitOfWork
        public MemberService(IUnitOfWork unitOfWork ,IMapper mapper , IAttachmentService attachmentService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _attachmentService = attachmentService; //DI Register
        }
        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {
            //Email Exist or Not
            var EmailExist =await _unitOfWork.GetRepository<Member>().AnyAsync(X => X.Email == model.Email);
            //Phone Exist Or Not
            var PhoneExist = await _unitOfWork.GetRepository<Member>().AnyAsync(X => X.Phone == model.Phone);

            if (EmailExist || PhoneExist) return false;

            //Upload Photo 
            var storedPhotoName = await _attachmentService.UploadAsync(model.PhotoFile.OpenReadStream() , model.PhotoFile.FileName , "MembersPhoto");
            if (string.IsNullOrWhiteSpace(storedPhotoName)) return false;


            //Add Member
            var member = _mapper.Map<Member>(model);
            member.Photo = storedPhotoName;


            _unitOfWork.GetRepository<Member>().Add(member);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            if (result > 0)
                return true;
            else
            {
                // Delete Photo
                _attachmentService.Delete(storedPhotoName, "MembersPhoto");
                return false;
            }
        }

        public async Task<bool> DeleteMemberAsync(int memberId, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(memberId, ct);
            if (member is null) return false;
            //If Member has Active Booking Or Not
            var HasActiveBooking = await _unitOfWork.GetRepository<Booking>().AnyAsync(B => B.MemberId == memberId && B.Session.StartDate > DateTime.Now); //Exception
            if(HasActiveBooking) return false;
            _unitOfWork.GetRepository<Member>().Delete(member);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
            
        }

        public async Task<IEnumerable<MemberViewModel>> GetAllAsync(CancellationToken ct = default)
        {

            //members come from Database
            var members = await _unitOfWork.GetRepository<Member>().GetAllAsync(ct:ct);
            if (!members.Any()) return [];
            //Member => ViewModel

            //List<MemberViewModel> memberVM = new List<MemberViewModel>();
            //foreach (var member in members) 
            //{
                //Data Comes From Database i need to send it to ViewModel
                //Manual Mapping
                var memberViewModel = _mapper.Map<IEnumerable<Member>, IEnumerable<MemberViewModel>>(members);
                //memberVM.Add(memberViewModel);
            //}
            return memberViewModel;
        }

 

        public async Task<MemberViewModel> GetMemberDetailsByIdAsync(int memberId, CancellationToken ct = default)
        {
            //Get Mmeber By Id
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(memberId, ct);
            if (member == null) return null;
            //Table = Member
            //Return =MemberViewModel
            var model = _mapper.Map<Member, MemberViewModel>(member);
            //check if the member has a ActiveMembership or not
            var ActiveMembership = await _unitOfWork.GetRepository<Membership>().FirstOrDefaultAsync(X => X.MemberId == memberId && X.EndDate > DateTime.Now  );
            if (ActiveMembership is not null)
            {
                var ActivePlan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(ActiveMembership.PlanId, ct);
                model.PlanName = ActivePlan.Name;
                model.MembershipStartDate = ActiveMembership.CreatedAt.ToString();
                model.MembershipEndDate = ActiveMembership.EndDate.ToString();
            }
            return model;
        }
        public async Task<HealthRecordViewModel> GetMemberHealthRecord(int memberId, CancellationToken ct)
        {
            var record = await _unitOfWork.GetRepository<HealthRecord>().FirstOrDefaultAsync(X => X.MemberId == memberId, ct: ct);

            if (record is null) return null;
            else
                return _mapper.Map<HealthRecord, HealthRecordViewModel>(record);
        }

        public async Task<MemberToUpdateViewModel> GetMemberToUpdateAsync(int memberId, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(memberId, ct);
            if (member is null) return null;
            else
                                   //src        //Destnation
                return _mapper.Map<Member, MemberToUpdateViewModel>(member); 
        }

        public async Task<bool> UpdateMemberAsync(int Id, MemberToUpdateViewModel model, CancellationToken ct = default)
        { 
            //Get Member
            var member =await _unitOfWork.GetRepository<Member>().GetByIdAsync(Id, ct);
            //Check If Any Other User Has The Same Email Or Phone Or Not
            var EmailExist = await _unitOfWork.GetRepository<Member>().AnyAsync(M => M.Email == model.Email && M.Id != Id);
            var PhoneExist = await _unitOfWork.GetRepository<Member>().AnyAsync(M => M.Phone == model.Phone && M.Id != Id);

            if(EmailExist || PhoneExist ) return false;
            _mapper.Map(model, member);
            member.UpdatedAt = DateTime.Now;

            _unitOfWork.GetRepository<Member>().Update(member);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;

        }
        
    }
}
