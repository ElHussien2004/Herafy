using Domain.Entities.UsersEntity;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    class TechnicianSpecifications:BaseSpecifications<Technician>
    {
        public TechnicianSpecifications(TechnicianQuery? query) 
            :base(T=>(!(query.ServiceId.HasValue)||T.ServiceCategoryId==query.ServiceId))
        {
            AddInclude(T => T.ServiceCategory);
            AddInclude(T=>T.User);
            if (query.ClientLatitude.HasValue && query.ClientLongitude.HasValue)
            {
                double lat = query.ClientLatitude.Value;
                double lon = query.ClientLongitude.Value;
                AddOrderBy(T =>
                    ((T.Latitude - lat) * (T.Latitude - lat)) +
                    ((T.Longitude - lon) * (T.Longitude - lon))
                );
            }
            switch (query.Sorting)
            {
                case TecnicianSorting.Rate:
                    AddOrderBy(T => T.RatingAvg);
                    break;
                case TecnicianSorting.Price:
                    AddOrderBy(T => T.InspectedPrice);
                    break;
                case TecnicianSorting.Available:
                    AddOrderBy(T=>T.AvailabilityStatus);
                    break;
                default:
                    break;
            }
        
        }
        public TechnicianSpecifications(string id):base(T=>T.Id==id)
        {
            AddInclude(T => T.ServiceCategory);
            AddInclude(T => T.User);
        }
    }
}
