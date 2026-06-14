using AutoMapper;
using GymManagement.DAL.Models;
using GymManagement.DAL.Models.Enums;
using GymManagement.DAL.Repositories.Interfaces;
using GymMangement.BLL.Services.Interfaces;
using GymMangement.BLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangement.BLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default)
        {
            if (model.EndDate <= model.StartDate) return false;
            if (model.StartDate <= DateTime.Now) return false;
            if (model.Capacity < 1 || model.Capacity > 25) return false;

            var trainer =await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId);
            if (trainer is null) return false;

            var category =await _unitOfWork.GetRepository<Category>().GetByIdAsync(model.CategoryId);
            if (category is null) return false;

            var isValid = Enum.TryParse<Specialties>(category.CategoryName, true ,out var CategorySpecialty);
            if (!isValid || trainer.Specialties != CategorySpecialty) return false;
            var session = _mapper.Map<CreateSessionViewModel, Session>(model);

            _unitOfWork.GetRepository<Session>().Add(session);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;

        }

        public async Task<IEnumerable<SessionViewModel>?> GetAllSessionsAsync(CancellationToken ct = default)
        {
            var sessionRepo = _unitOfWork.SessionRepository;
            var sessions = await sessionRepo.GetAllSessionswithTrainerAndCategory(ct);
            if (sessions == null || !sessions.Any()) return null;

            var mappedSessions = sessions.Select(S => new SessionViewModel()
            {
                Id = S.Id,
                Capacity = S.Capacity,
                CategoryName = S.Category.CategoryName,
                TrainerName = S.Trainer.Name,
                Description = S.Description,
                EndDate = S.EndDate,
                StartDate = S.StartDate,

            });
            foreach (var session in mappedSessions)
            {
                session.AvailableSlots = session.Capacity - await sessionRepo.GetCountOfBookedSlotAsync(session.Id, ct);
                //N + 1 problem
            }
            return mappedSessions;
        }

        public async Task<IEnumerable<CategorySelectViewModel>> GetCategoryForDropDownAsync(CancellationToken ct = default)
        {
            var result =await _unitOfWork.GetRepository<Category>().GetAllAsync(ct: ct);
            return _mapper.Map<IEnumerable<CategorySelectViewModel>>(result);
        }

        public async Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForDropDownAsync(CancellationToken ct = default)
        {
            var result = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(ct: ct);
            return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(result);
        }
    }
}
