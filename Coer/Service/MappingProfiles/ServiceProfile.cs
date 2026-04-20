using AutoMapper;
using Domain.Entities.ServiceEntity;
using Shared.DTOs.TechnicianDTOS.ServiceCategry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.MappingProfiles
{
    public class ServiceProfile:Profile
    {
        public ServiceProfile()
        {
            CreateMap<ServiceCategory,ServiceDto>()
                .ForMember(des=>des.Name ,sr=>sr.MapFrom(s=>s.Name));
        }
    }
}
