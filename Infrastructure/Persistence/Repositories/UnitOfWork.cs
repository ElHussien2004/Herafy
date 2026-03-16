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
        readonly private Lazy<IGenericRepository<Technician, string>> _technicalRepo = new Lazy<IGenericRepository<Technician, string>>(()=>new GenericRepository<Technician,string>(_Context));
        public IGenericRepository<Technician, string> TechnicalRepository => _technicalRepo.Value;
        readonly private Lazy<IGenericRepository<Client, string>> _clientRepo = new Lazy<IGenericRepository<Client, string>>(() => new GenericRepository<Client, string>(_Context));
        public IGenericRepository<Client, string> ClientRepository => _clientRepo.Value;
        public Task<int> SaveAsync()
        {
          return _Context.SaveChangesAsync();
        }
    }
}
