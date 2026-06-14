using AutoMapper;
using GymManagement.DAL.Models;
using GymMangement.BLL.ViewModels;
using GymMangement.BLL.ViewModels.MemberViewModels;
using GymMangement.BLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GymMangement.BLL
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            MapMember();
            MapSession();
        }
        private void MapMember()
        {
            CreateMap<Member, MemberViewModel>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToShortDateString()));

            CreateMap<HealthRecord, HealthRecordViewModel>().ReverseMap();

            CreateMap<Member, MemberToUpdateViewModel>()
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City));

            CreateMap<MemberToUpdateViewModel, Member>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.Photo, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.Address.BuildingNumber = src.BuildingNumber;
                    dest.Address.Street = src.Street;
                    dest.Address.City = src.City;
                });

            CreateMap<CreateMemberViewModel, Member>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address()
                {
                    BuildingNumber = src.BuildingNumber,
                    Street = src.Street,
                    City = src.City,
                }))
                .ForMember(dest => dest.HealthRecord, opt => opt.MapFrom(src => src.HealthRecordViewModel));
        }
        private void MapSession()
        {
            CreateMap<CreateSessionViewModel, Session>();
        }
    }
}
