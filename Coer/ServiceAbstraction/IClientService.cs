using Shared.DTOs.ClientDTOS;
using Shared.DTOs.TechnicianDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
   public interface IClientService
    {
        Task<IEnumerable<ClientDto>> GetAllAsync();
        Task<ClientDto?> GetByIdAsync(string id);
        Task AddAsync(AddClientDto clientDto);
        Task<bool> UpdateAsync(string id, UpdateClientDto clientDto);
        Task<bool> DeleteAsync(string id);
    }
}