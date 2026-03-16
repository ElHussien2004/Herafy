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
        Task<bool> UploadDocumentsAsync(string technicianId, UploadDocumentsDto documents);
        Task<bool> ToggleAvailabilityStatusAsync(string technicianId, bool isAvailable);
    }
}
