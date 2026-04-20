using AutoMapper;
using Domain.Entities.UsersEntity;
using Shared.DTOs.ClientDTOS;
using Shared.DTOs.TechnicianDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.MappingProfiles
{
   public class ClientProfile:Profile
    {
        public ClientProfile()
        {
            CreateMap<AddClientDto, Client>();

            CreateMap<Client,ClientDto>()
                .ForMember(d => d.ProfileImageURL,
                    opt => opt.MapFrom<URLResolver<Client, ClientDto>, string?>(
                        s => s.User != null ? s.User.ProfileImageURL : null))
                .ForMember(d => d.FullName,
                    opt => opt.MapFrom(s => s.User != null ? s.User.FullName : null))
                .ForMember(des=>des.UserId ,sr=>sr.MapFrom(s=>s.Id))
                .ForMember(des=>des.PhoneNumber ,sr=>sr.MapFrom(s=>s.User.PhoneNumber))
                .ForMember(des=>des.NumberOfOrder,sr=>sr.MapFrom(s=>s.Orders.Count()))
                .ForMember(des=>des.CreatedAt,sr=>sr.MapFrom(s=>s.CreatedAt));

            CreateMap<Client, ClientDetailsDto>()
               .ForMember(d => d.ProfileImageURL,
                   opt => opt.MapFrom<URLResolver<Client, ClientDetailsDto>, string?>(
                       s => s.User != null ? s.User.ProfileImageURL : null))
               .ForMember(d => d.FullName,
                   opt => opt.MapFrom(s => s.User != null ? s.User.FullName : null))
               .ForMember(des => des.UserId, sr => sr.MapFrom(s => s.Id))
               .ForMember(des => des.PhoneNumber, sr => sr.MapFrom(s => s.User.PhoneNumber))
               .ForMember(des => des.NumberOfOrder, sr => sr.MapFrom(s => s.Orders.Count()))
               .ForMember(des => des.CreatedAt, sr => sr.MapFrom(s => s.CreatedAt));

        }
    }
}
