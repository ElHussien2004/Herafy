using Domain.Entities.UsersEntity;
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
        Task<Result<IEnumerable<TechnicianDto>>> GetAllAsync();
        Task<Result<TechnicialDetailsDto>> GetByIdAsync(string id);
        Task<Result> AddAsync(AddTechnicianDto technician);
        Task UpdateAsync(UpdateTechnicianDto technician);
        Task<bool> DeleteAsync(string id);
        Task<bool> UploadDocumentsAsync(string technicianId, UploadDocumentsDto documents);
        Task<bool> ToggleAvailabilityStatusAsync(string technicianId, bool isAvailable); 
    }

  
}
