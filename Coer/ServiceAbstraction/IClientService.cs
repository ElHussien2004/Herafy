using Shared.CommonResult;
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
        Task<Result<IEnumerable<ClientDto>>> GetAllAsync(string? search);
        public Task<Result<int>> CountAsync();
        Task<Result> UpdateAsync(string id, UpdataClientdto dto);
        public Task<Result<bool>> ChangeIsActive(string id, bool state);
        
        Task<Result<ClientDetailsDto>> GetByIdAsync(string id);
        Task <Result> AddAsync(string Userid,AddClientDto clientDto);
       
        Task<Result<bool>> DeleteAsync(string id);
    }
}