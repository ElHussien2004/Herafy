using Shared.CommonResult;
using Shared.DTOs.ComplaintDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface IComplaintService
    {
        Task<Result> CreateComplaintAsync(CreateComplaintDto dto);

        Task<Result<List<GetAllComplaintDto>>> GetAllComplaintsAsync();

        Task<Result<GetDetailsComplaintDto>> GetComplaintByIdAsync(int id);

        Task<Result<ComplaintResponseDto>> RespondToComplaintAsync(int complaintId, string response);

        Task<Result<bool>> DeleteComplaintAsync(int id);
    }
}
