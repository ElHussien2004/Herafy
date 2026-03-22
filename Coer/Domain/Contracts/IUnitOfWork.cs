using Domain.Entities;
using Domain.Entities.UsersEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IUnitOfWork
    {
        public IGenericRepository<Technician,string> TechnicalRepository { get; }
        public IGenericRepository<Client, string> ClientRepository { get; }

        Task<int> SaveAsync();

    }
}
