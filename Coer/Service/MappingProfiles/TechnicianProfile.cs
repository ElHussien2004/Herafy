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
            CreateMap<Technician, TechnicianDto>().ReverseMap();
            CreateMap<Technician, AddTechnicianDto>().ReverseMap();
            CreateMap<Technician, UpdateTechnicianDto>().ReverseMap();
            CreateMap<Technician, UploadDocumentsDto>().ReverseMap();
        }
    }
}
