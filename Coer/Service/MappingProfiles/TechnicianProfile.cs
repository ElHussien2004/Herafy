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
            CreateMap<Technician, TechniciaDetailsDto>()
                .ForMember(d => d.ServiceCategory,
                    opt => opt.MapFrom(s => s.ServiceCategory != null ? s.ServiceCategory.Name : null))
                .ForMember(d => d.Fullname,
                    opt => opt.MapFrom(s => s.User != null ? s.User.FullName : null))
                .ForMember(d => d.ProfileImageURL,
                    opt => opt.MapFrom<URLResolver<Technician, TechniciaDetailsDto>, string?>(
                        s => s.User != null ? s.User.ProfileImageURL : null));

            CreateMap<Technician, TechnicialDto>()
                .ForMember(d => d.ServiceCategory,
                    opt => opt.MapFrom(s => s.ServiceCategory != null ? s.ServiceCategory.Name : null))
                .ForMember(d => d.Fullname,
                    opt => opt.MapFrom(s => s.User != null ? s.User.FullName : null))
                .ForMember(d => d.ProfileImageURL,
                    opt => opt.MapFrom<URLResolver<Technician, TechnicialDto>, string?>(
                        s => s.User != null ? s.User.ProfileImageURL : null
                    ));

            CreateMap< AddTechnicianDto,Technician>();
             

            CreateMap<Technician,GetDocumentDto>()
                .ForMember(d => d.ServiceCategory,
                    opt => opt.MapFrom(s => s.ServiceCategory != null ? s.ServiceCategory.Name : null))
                .ForMember(d => d.FullName,
                    opt => opt.MapFrom(s => s.User != null ? s.User.FullName : null))
                .ForMember(d => d.ProfileImageURL,
                    opt => opt.MapFrom<URLResolver<Technician, GetDocumentDto>, string?>(
                        s => s.User != null ? s.User.ProfileImageURL : null
                    ))
                .ForMember(d=>d.PhoneNumber ,sr=>sr.MapFrom(s=>s.User.PhoneNumber))
                .ForMember(d => d.FaceImageUrl,
                    opt => opt.MapFrom<URLResolver<Technician, GetDocumentDto>, string?>(
                        s => s.Document != null ? s.Document.FaceImageUrl : null
                    )).ForMember(d => d.BackImageUrl,
                    opt => opt.MapFrom<URLResolver<Technician, GetDocumentDto>, string?>(
                        s => s.Document != null ? s.Document.BackImageUrl : null
                    ));



        }
    }
}
