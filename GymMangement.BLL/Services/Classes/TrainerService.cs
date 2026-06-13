using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using GymMangement.BLL.Services.Interfaces;
using GymMangement.BLL.ViewModels.TrainerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangement.BLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        //Connection
        private readonly IGenericRepository<Trainer> _trainerRepository;
        private readonly IGenericRepository<Session> _sessionRepository;

        public TrainerService(IGenericRepository<Trainer> trainerRepository , IGenericRepository<Session> sessionRepository)
        {
            _trainerRepository = trainerRepository;
            _sessionRepository = sessionRepository;
        }
        public async Task<bool> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken ct = default)
        {
            if (await _trainerRepository.AnyAsync(t => t.Email == model.Email, ct)) return false;
            if (await _trainerRepository.AnyAsync(t => t.Phone == model.Phone, ct)) return false;

            var trainer = new Trainer()
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Specialties = model.Specialties,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                Address = new Address ()
                {
                    City = model.City,
                    BuildingNumber = model.BuildingNumber,
                    Street = model.Street
                }
            };
            var result = await _trainerRepository.AddAsync(trainer, ct);
            return result > 0 ;
        }

        public async Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct = default)
        {
            var trainers = await _trainerRepository.GetAllAsync(ct: ct);
            return trainers.Select(t => new TrainerViewModel()
            {
                Id = t.Id,
                Name = t.Name,
                Email = t.Email,
                Phone = t.Phone,
                Specialties = t.Specialties.ToString()
            });
        }

        public async Task<TrainerViewModel> GetTrainerDetailsAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await _trainerRepository.GetByIdAsync(trainerId, ct);
            if (trainer == null) return null;
            else return new TrainerViewModel()
            {
                Name = trainer.Name,
                Specialties = trainer.Specialties.ToString(),
                Email = trainer.Email,
                Phone = trainer.Phone,
                DateOfBirth = trainer.DateOfBirth.ToShortDateString(),
                Address = $"{trainer.Address.BuildingNumber}-{trainer.Address.Street}-{trainer.Address.City}"
            };
        }

        public async Task<TrainerToUpdateViewModel> GetTrainerToUpdateAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await _trainerRepository.GetByIdAsync(trainerId, ct);
            if (trainer == null) return null;
            else
                return new TrainerToUpdateViewModel()
                {
                    Name = trainer.Name,
                    Email = trainer.Email,
                    Phone = trainer.Phone,
                    BuildingNumber = trainer.Address.BuildingNumber,
                    Street = trainer.Address.Street,
                    City = trainer.Address.City,
                    Specialties = trainer.Specialties
                };
        }

        public async Task<bool> RemoveTrainerAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await _trainerRepository.GetByIdAsync(trainerId, ct);
            if(trainer is null ) return false;

            var hasFutureSession = await _sessionRepository.AnyAsync(s => s.TrainerId == trainerId && s.StartDate > DateTime.Now);
            if (hasFutureSession)
                return false;
            var result = await _trainerRepository.DeleteAsync(trainer, ct);
            return result > 0;
        }

        public async Task<bool> UpdateTrainerDetailsAsync(int trainerId, TrainerToUpdateViewModel model, CancellationToken ct = default)
        {
            var trainer = await _trainerRepository.GetByIdAsync(trainerId, ct);
            if (trainer == null) return false;
            if (await _trainerRepository.AnyAsync(t => t.Email == model.Email && t.Id != trainerId, ct)) return false;
            if (await _trainerRepository.AnyAsync(t => t.Phone == model.Phone && t.Id != trainerId, ct)) return false;

            trainer.Email = model.Email;
            trainer.Phone = model.Phone;
            trainer.Address.City = model.City;
            trainer.Address.Street = model.Street;
            trainer.Address.BuildingNumber = model.BuildingNumber;
            trainer.Specialties = model.Specialties;
            trainer.UpdatedAt = DateTime.Now;

            var result = await _trainerRepository.UpdateAsync(trainer,ct);
            return result > 0;

        }
    }
}
