using AutoMapper;
using Domain.Entities.OrderEntity;
using Shared.DTOs.ClientDTOS;
using Shared.DTOs.ReviewDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.MappingProfiles
{
    public class ReviewProfile:Profile
    {
        public ReviewProfile()
        {
            CreateMap<Review, GetTechnicianReviews>()
                .ForMember(des=>des.Id,sr=>sr.MapFrom(s=>s.Id))
                .ForMember(des => des.Comment, sr => sr.MapFrom(s => s.Comment))
                .ForMember(des => des.NameClient, sr => sr.MapFrom(s => s.Order.Client.User.FullName))
                .ForMember(d => d.ImageURLClient,
                   opt => opt.MapFrom<URLResolver<Review, GetTechnicianReviews>, string?>(
                       s => s.Order.Client.User != null ? s.Order.Client.User.ProfileImageURL : null))
                .ForMember(des => des.Rating, sr => sr.MapFrom(s => s.Rating))
                ;
            CreateMap<Review, GetAllReviewsDTO>()
               .ForMember(d => d.NameClient, o => o.MapFrom(s => s.Order.Client.User.FullName))
               .ForMember(d => d.NameTechnician, o => o.MapFrom(s => s.Order.Technician.User.FullName));

            CreateMap<Review, GetDetailsReviewAdmin>()
               .ForMember(d => d.NameClient, o => o.MapFrom(s => s.Order.Client.User.FullName))
               .ForMember(d => d.NameTechnician, o => o.MapFrom(s => s.Order.Technician.User.FullName))
               .ForMember(d => d.ConfidenceScore, o => o.MapFrom(s => s.ConfidenceScore))
               .ForMember(d=>d.is_suspicious,sr=>sr.MapFrom(s=>s.is_suspicious))
               .ForMember(d=>d.FraudReasons,sr=>sr.MapFrom(s=>s.FraudReasons))
               .ForMember(d=>d.Comment,sr=>sr.MapFrom(s=>s.Comment))
                ;
        }
    }
}
