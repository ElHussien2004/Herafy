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
        Task<Result<IEnumerable<ClientDto>>> GetAllAsync();
        public Task<Result<int>> CountAsync();
        Task<Result> UpdateAsync(string id, UpdataClientdto dto);
        Task<Result<ClientDto>> GetByIdAsync(string id);
        Task <Result> AddAsync(string Userid,AddClientDto clientDto);
        Task<Result<bool>> ChangeIsActive(string id ,bool State);
        Task<Result<bool>> DeleteAsync(string id);
    }
}