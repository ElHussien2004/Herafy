using AutoMapper;
using Domain.Contracts;
using Domain.Entities.UsersEntity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using ServiceAbstraction;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ServiceManager(IConfiguration configuration,IUnitOfWork unitOfWork,IFileService fileService,IMapper mapper,IConnectionMultiplexer connection,ISMSService sMS,UserManager<ApplicationUser> userManager ) : IServiceManager
    {
        private readonly Lazy<ISMSService> _LazysmsService = new Lazy<ISMSService>(() => new SMSService(configuration));
        public ISMSService SMSService =>_LazysmsService.Value;
        private readonly Lazy<IFileService> _LazyFileService = new Lazy<IFileService>(() => new FileService());

        public IFileService FileService => _LazyFileService.Value;

        private readonly Lazy<ITechnicianService> _LazyTechnicianService = new Lazy<ITechnicianService>(() => new TechnicianService(mapper,unitOfWork, fileService, userManager));

        public ITechnicianService TechnicianService => _LazyTechnicianService.Value;

        private readonly Lazy<IAuthService> _LazyAuthService = new Lazy<IAuthService>(() => new AuthService(connection,sMS));

        public IAuthService AuthService => _LazyAuthService.Value;
    }
}
