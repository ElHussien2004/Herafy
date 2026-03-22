using Domain.Contracts;
using Domain.Entities.UsersEntity;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class UnitOfWork(HerafyDbContext _Context) : IUnitOfWork
    {
        private readonly Lazy<IGenericRepository<Technician, string>> _technicalRepo =
          new(() => new GenericRepository<Technician, string>(_Context));
        public IGenericRepository<Technician, string> TechnicalRepository => _technicalRepo.Value;

        private readonly Lazy<IGenericRepository<Client, string>> _clientRepo =
            new(() => new GenericRepository<Client, string>(_Context));
        public IGenericRepository<Client, string> ClientRepository => _clientRepo.Value;
        public Task<int> SaveAsync() => _Context.SaveChangesAsync();
    }
}
