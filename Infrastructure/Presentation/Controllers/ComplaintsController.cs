using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared;
using Shared.DTOs.ComplaintDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class ComplaintsController(IServiceManager serviceManager) : ApiBaseController
    {
        [HttpPost("CreateComplaint")]
        [Authorize(Roles = $"{Roles.Client},{Roles.Technician}")]
        public async Task<IActionResult> CreateComplaint(CreateComplaintDto dto)
        {
            var result = await serviceManager.ComplaintService.CreateComplaintAsync(dto);
            return HandleResult(result);
        }

        [HttpGet("GetAllComplaints")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<List<GetAllComplaintDto>>> GetAllComplaints()
        {
            var result = await serviceManager.ComplaintService.GetAllComplaintsAsync();
            return HandleResult(result);
        }

        [HttpGet("GetComplaintDetails")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<GetDetailsComplaintDto>> GetComplaintDetails([FromQuery]int id)
        {
            var result = await serviceManager.ComplaintService.GetComplaintByIdAsync(id);
            return HandleResult(result);
        }

        [HttpPatch("RespondToComplaint")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<ComplaintResponseDto>> RespondToComplaint([FromQuery]int complaintId, [FromBody] string response)
        {
            var result = await serviceManager.ComplaintService.RespondToComplaintAsync(complaintId, response);
            return HandleResult(result);
        }

        [HttpDelete("DeleteComplaint")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<bool>> DeleteComplaint([FromQuery]int id)
        {
            var result = await serviceManager.ComplaintService.DeleteComplaintAsync(id);
            return HandleResult(result);
        }
    }
}
