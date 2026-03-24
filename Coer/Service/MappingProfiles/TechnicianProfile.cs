using AutoMapper;
using Domain.Entities.UsersEntity;
using Shared.DTOs.TechnicianDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Service.MappingProfiles
{
    public class TechnicianProfile : Profile
    {
        public TechnicianProfile()
        {
            CreateMap<Technician, TechnicianDto>()
            .ForMember(des => des.ServiceCategory, sr => sr.MapFrom(t => t.ServiceCategory.Name))
            .ForMember(des=>des.Fullname,op=>op.MapFrom(sr=>sr.User.FullName))
            .ForMember(des=>des.ProfileImageURL,op=>op.MapFrom(sr=>sr.User.ProfileImageURL));

            CreateMap<Technician, TechnicialDetailsDto>()
               .ForMember(des => des.ServiceCategory, sr => sr.MapFrom(t => t.ServiceCategory.Name))
              .ForMember(des => des.Fullname, op => op.MapFrom(sr => sr.User.FullName))
             .ForMember(des => des.ProfileImageURL, op => op.MapFrom(sr => sr.User.ProfileImageURL));

            CreateMap< AddTechnicianDto,Technician>();
            CreateMap<Technician, UpdateTechnicianDto>().ReverseMap();
            CreateMap<Technician, UploadDocumentsDto>().ReverseMap();
        }
    }
}
