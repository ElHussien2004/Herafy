using Domain.Entities.UsersEntity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface IServiceManager
    {
         public ISMSService SMSService { get; }
         public IFileService FileService { get; }
         public ITechnicianService TechnicianService { get; }
         public IAuthService AuthService { get; }
        public IClientService ClientService { get; }
        public IOrderService OrderService { get; }

        public IReviewService ReviewService { get; }
    }
}
