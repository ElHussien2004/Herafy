using AutoMapper;
using Domain.Entities.OrderEntity;
using Domain.Entities.UsersEntity;
using Shared.DTOs.OrderDtos;
using Shared.DTOs.TechnicianDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.MappingProfiles
{
    public class OrderProfile:Profile
    {
        public OrderProfile()
        {
            CreateMap<CreateOrderDto, Order>().
                ForMember(des => des.ClientId, sr => sr.MapFrom(s => s.ClientId))
                .ForMember(des => des.TechnicianId, sr => sr.MapFrom(s => s.TechnicianId))
                .ForMember(des => des.ServiceId, sr => sr.MapFrom(s => s.ServiceId))
                .ForMember(des => des.City, sr => sr.MapFrom(s => s.City))
                .ForMember(des => des.Government, sr => sr.MapFrom(s => s.Government))
                .ForMember(des => des.ProblemDetails, sr => sr.MapFrom(s => s.ProblemDetails))
                .ForMember(des => des.PlaceDetails, sr => sr.MapFrom(s => s.PlaceDetails))
                .ForMember(des => des.ScheduledDate, sr => sr.MapFrom(s => s.ScheduledDate))
                .ForMember(des => des.ScheduledTime, sr => sr.MapFrom(s => s.ScheduledTime))
                .ForMember(des => des.InspectedPrice, sr => sr.MapFrom(s => s.InspectedPrice));

            CreateMap<Order, GetDetailsOrderClientDTO>()
                .ForMember(des => des.Id, sr => sr.MapFrom(s => s.Id))
                .ForMember(des => des.ServiceName, sr => sr.MapFrom(s => s.ServiceCategory.Name))
                .ForMember(des => des.ScheduledDate, sr => sr.MapFrom(s => s.ScheduledDate))
                .ForMember(des => des.ScheduledTime, sr => sr.MapFrom(s => s.ScheduledTime))
                .ForMember(des => des.InspectedPrice, sr => sr.MapFrom(s => s.InspectedPrice))
                .ForMember(des => des.ProblemDetails, sr => sr.MapFrom(s => s.ProblemDetails))
                .ForMember(des => des.PlaceDetails, sr => sr.MapFrom(s => s.PlaceDetails))
                .ForMember(des => des.City, sr => sr.MapFrom(s => s.City))
                .ForMember(des => des.AfterPrice, sr => sr.MapFrom(s => s.FinalPrice - s.InspectedPrice))
                 .ForMember(des => des.FinalPrice, sr => sr.MapFrom(s => s.FinalPrice))
                .ForMember(des => des.Government, sr => sr.MapFrom(s => s.Government))
                .ForMember(des => des.ImageTecURL,
                    opt => opt.MapFrom<URLResolver<Order, GetDetailsOrderClientDTO>, string?>(
                        s => s.Technician != null ? s.Technician.User.ProfileImageURL : null))
                .ForMember(des => des.NameTechnician, sr => sr.MapFrom(s => s.Technician.User.FullName))
                .ForMember(des => des.RatingAvg, sr => sr.MapFrom(s => s.Technician.RatingAvg))
                .ForMember(des => des.State, sr => sr.MapFrom(s => s.Status.ToString()));

            CreateMap<Order, GetDetailsOrderTechnicianDTO>()
                 .ForMember(des => des.Id, sr => sr.MapFrom(s => s.Id))
                 .ForMember(des => des.ServiceName, sr => sr.MapFrom(s => s.ServiceCategory.Name))
                 .ForMember(des => des.ScheduledDate, sr => sr.MapFrom(s => s.ScheduledDate))
                 .ForMember(des => des.ScheduledTime, sr => sr.MapFrom(s => s.ScheduledTime))
                 .ForMember(des => des.InspectedPrice, sr => sr.MapFrom(s => s.InspectedPrice))
                 .ForMember(des => des.AfterPrice, sr => sr.MapFrom(s => s.FinalPrice - s.InspectedPrice))
                 .ForMember(des => des.FinalPrice, sr => sr.MapFrom(s => s.FinalPrice))
                 .ForMember(des => des.NameClient, sr => sr.MapFrom(s => s.Client.User.FullName))
                 .ForMember(des => des.State, sr => sr.MapFrom(s => s.Status.ToString()))
                 .ForMember(des => des.ImageCliURL,
                    opt => opt.MapFrom<URLResolver<Order, GetDetailsOrderTechnicianDTO>, string?>(
                        s => s.Client != null ? s.Client.User.ProfileImageURL : null));


            CreateMap<Order, GetDetailsOrderAdminDTO>()
                .ForMember(des => des.Id, sr => sr.MapFrom(s => s.Id))
                .ForMember(des => des.ServiceName, sr => sr.MapFrom(s => s.ServiceCategory.Name))
                .ForMember(des => des.ScheduledDate, sr => sr.MapFrom(s => s.ScheduledDate))
                .ForMember(des => des.ScheduledTime, sr => sr.MapFrom(s => s.ScheduledTime))
                .ForMember(des => des.InspectedPrice, sr => sr.MapFrom(s => s.InspectedPrice))
                .ForMember(des => des.AfterPrice, sr => sr.MapFrom(s => s.FinalPrice - s.InspectedPrice))
                .ForMember(des => des.FinalPrice, sr => sr.MapFrom(s => s.FinalPrice))
                .ForMember(des => des.NameClient, sr => sr.MapFrom(s => s.Client.User.FullName))
                .ForMember(des => des.State, sr => sr.MapFrom(s => s.Status.ToString()))
                .ForMember(des => des.ImageCliURL,
                   opt => opt.MapFrom<URLResolver<Order, GetDetailsOrderAdminDTO>, string?>(
                       s => s.Client != null ? s.Client.User.ProfileImageURL : null))
                .ForMember(des => des.ImageTecUrl,
                    opt => opt.MapFrom<URLResolver<Order, GetDetailsOrderAdminDTO>, string?>(
                        s => s.Technician != null ? s.Technician.User.ProfileImageURL : null))
                 .ForMember(des => des.NameTec, sr => sr.MapFrom(s => s.Technician.User.FullName))
                .ForMember(des => des.RatingAvg, sr => sr.MapFrom(s => s.Technician.RatingAvg))
                .ForMember(des => des.State, sr => sr.MapFrom(s => s.Status.ToString()));

            CreateMap<Order, GetAllOrderDTO>()
                .ForMember(des => des.NameCli, sr => sr.MapFrom(s => s.Client.User.FullName))
                .ForMember(des => des.NameTec, sr => sr.MapFrom(s => s.Technician.User.FullName))
                 .ForMember(des => des.ServiceName, sr => sr.MapFrom(s => s.ServiceCategory.Name))
                  .ForMember(des => des.FinalPrice, sr => sr.MapFrom(s => s.FinalPrice))
                  .ForMember(des => des.State, sr => sr.MapFrom(s => s.Status.ToString()))
                  .ForMember(des => des.Id, sr => sr.MapFrom(s => s.Id))
                  .ForMember(des=>des.CreatedAt,sr=>sr.MapFrom(s=>s.CreatedAt));

            CreateMap<Order, GetTechnicianOrder>()
                 .ForMember(des => des.Id, sr => sr.MapFrom(s => s.Id))
                .ForMember(des => des.ServiceName, sr => sr.MapFrom(s => s.ServiceCategory.Name))
                .ForMember(des => des.ScheduledDate, sr => sr.MapFrom(s => s.ScheduledDate))
                .ForMember(des => des.ScheduledTime, sr => sr.MapFrom(s => s.ScheduledTime))
                .ForMember(des => des.InspectedPrice, sr => sr.MapFrom(s => s.InspectedPrice))
                .ForMember(des => des.State, sr => sr.MapFrom(s => s.Status.ToString()))
                .ForMember(des=>des.ProblemDetails,sr=>sr.MapFrom(s=>s.ProblemDetails))
                .ForMember(des=>des.PlaceDetails,sr=>sr.MapFrom(s=>s.PlaceDetails));

            CreateMap<Order, GetClientOrderDTO>()
               .ForMember(des => des.Id, sr => sr.MapFrom(s => s.Id))
               .ForMember(des => des.ServiceName, sr => sr.MapFrom(s => s.ServiceCategory.Name))
               .ForMember(des => des.ScheduledDate, sr => sr.MapFrom(s => s.ScheduledDate))
               .ForMember(des => des.ScheduledTime, sr => sr.MapFrom(s => s.ScheduledTime))
               .ForMember(des => des.InspectedPrice, sr => sr.MapFrom(s => s.InspectedPrice))
               .ForMember(des => des.State, sr => sr.MapFrom(s => s.Status.ToString()));
              
        }
    }
}
