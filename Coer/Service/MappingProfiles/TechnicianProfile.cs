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
                .ForMember(d => d.ServiceCategory,
                    opt => opt.MapFrom(s => s.ServiceCategory != null ? s.ServiceCategory.Name : null))
                .ForMember(d => d.Fullname,
                    opt => opt.MapFrom(s => s.User != null ? s.User.FullName : null))
                .ForMember(d => d.ProfileImageURL,
                    opt => opt.MapFrom<URLResolver<Technician, TechnicianDto>, string?>(
                        s => s.User != null ? s.User.ProfileImageURL : null));

            CreateMap<Technician, TechnicialDetailsDto>()
                .ForMember(d => d.ServiceCategory,
                    opt => opt.MapFrom(s => s.ServiceCategory != null ? s.ServiceCategory.Name : null))
                .ForMember(d => d.Fullname,
                    opt => opt.MapFrom(s => s.User != null ? s.User.FullName : null))
                .ForMember(d => d.ProfileImageURL,
                    opt => opt.MapFrom<URLResolver<Technician, TechnicialDetailsDto>, string?>(
                        s => s.User != null ? s.User.ProfileImageURL : null
                    ));

            CreateMap< AddTechnicianDto,Technician>();
       
          
        }
    }
}
