using Domain.Entities;
using Domain.Entities.OrderEntity;
using Domain.Entities.ServiceEntity;
using Domain.Entities.UsersEntity;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IUnitOfWork
    {
        public IGenericRepository<Technician,string> TechnicalRepository { get; }
        public IGenericRepository<Client, string> ClientRepository { get; }

        public IGenericRepository<TechnicianDocument, string> DocumentRepository { get; }
        public IGenericRepository<ServiceCategory, int> ServiceCategoryRepository { get; }
        public IGenericRepository<Order , int> OrderRepo { get; }
        public IGenericRepository<Review, int> ReviewRepo { get; }
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<int> SaveAsync();

    }
}
