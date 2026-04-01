using Domain.Entities.UsersEntity;
using Shared;
using Shared.CommonResult;
using Shared.DTOs.TechnicianDTOS;
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
        Task<Result<TechnicialDetailsDto>> GetByIdAsync(string id);
        Task<Result> AddAsync(AddTechnicianDto technician);
        Task<Result> UpdateAsync(UpdateTechnicianDto technician);
        Task<Result<bool>> DeleteAsync(string id);
        Task<Result<bool>> UploadDocumentsAsync(string technicianId, UploadDocumentsDto documents);
        Task<Result<bool>> ToggleAvailabilityStatusAsync(string technicianId, bool isAvailable); 
    }

  
}
