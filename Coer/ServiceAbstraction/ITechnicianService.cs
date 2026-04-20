using Domain.Entities.UsersEntity;
using Shared;
using Shared.CommonResult;
using Shared.DTOs.TechnicianDTOS;
using Shared.DTOs.TechnicianDTOS.ServiceCategry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface ITechnicianService
    {
        Task<Result<IEnumerable<TechnicianDto>>> GetAllAsync(TechnicianQuery ParamsQuery);
        Task<Result<IEnumerable<ServiceDto>>>GetAllService();
        Task<Result<TechniciaDetailsDto>> GetByIdAsync(string id);
        Task<Result> AddAsync(string Id,AddTechnicianDto technician);
        Task<Result> UpdateAsync(string id,UpdateTechnicianDto technician);
        Task<Result<bool>> DeleteAsync(string id);
        public Task<Result<int>> CountAsync();
        Task<Result<bool>> UploadDocumentsAsync(string technicianId, UploadDocumentsDto documents);
        Task<Result<bool>> ToggleAvailabilityStatusAsync(string technicianId, bool isAvailable);
        Task<Result<bool>> ChangeIsActive(string id, bool State);

        Task<Result<GetDocumentDto>> GetDocument(string id);

    }

  
}
