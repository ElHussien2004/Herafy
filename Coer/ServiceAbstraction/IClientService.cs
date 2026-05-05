using Domain.Entities.UsersEntity;
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
        public Task<Result<bool>> ChangeIsActive(string id, StateUser stateUser);
        
        Task<Result<ClientDetailsDto>> GetByIdAsync(string id);
        Task <Result> AddAsync(string Userid,AddClientDto clientDto);
       
        Task<Result<bool>> DeleteAsync(string id);
        Task<Result<bool>> UploadDocumentsAsync(string ClientId, UploadDocumentsDto documents);
        Task<Result<GetDecumentClient>> GetDocument(string id);
    }
}