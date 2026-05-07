using AutoMapper;
using Domain.Entities.OrderEntity;
using Shared.DTOs.ComplaintDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.MappingProfiles
{
    
    public class ComplaintProfile : Profile
    {
        public ComplaintProfile()
        {
            // Create Complaint
            CreateMap<CreateComplaintDto, Complaint>();

            // Get All Complaints
            CreateMap<Complaint, GetAllComplaintDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.UserRole, opt => opt.Ignore())
                .ForMember(des=>des.Status,sr=>sr.MapFrom(s=>s.Status.ToString()));

            // Get Complaint Details
            CreateMap<Complaint, GetDetailsComplaintDto>()
               .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName))
               .ForMember(dest => dest.UserRole, opt => opt.Ignore())
               .ForMember(des => des.Status, sr => sr.MapFrom(s => s.Status.ToString()))
               .ForMember(des=>des.PhoneNumber,sr=>sr.MapFrom(s=>s.User.PhoneNumber))
               ;


            // Response DTO
            CreateMap<Complaint, ComplaintResponseDto>()
                .ForMember(dest => dest.ComplaintId, opt => opt.MapFrom(src => src.Id))
                .ForMember(des => des.Status, sr => sr.MapFrom(s => s.Status.ToString()));
        }

    }
}
