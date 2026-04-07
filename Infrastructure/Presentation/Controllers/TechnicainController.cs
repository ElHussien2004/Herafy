using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared;
using Shared.DTOs.TechnicianDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class TechnicainController(IServiceManager _serviceManager):ApiBaseController
    {
        [HttpGet("Profile")]
        public async Task<ActionResult<TechniciaDetailsDto>> GetProfile()
        {
            var result = await _serviceManager.TechnicianService.GetByIdAsync(GetUserId());
            return  HandleResult(result);
        }

        //  Add New Technician (uses Form due to Profile Image)
        [HttpPost("AddTechnician")]
        public async Task<IActionResult> AddTechnician([FromForm] AddTechnicianDto technicianDto)
        {
            var result = await _serviceManager.TechnicianService.AddAsync(GetUserId(),technicianDto);
            return HandleResult(result);
        }

        [HttpPatch("Update")]
        public async Task<IActionResult> Update([FromForm] UpdateTechnicianDto dto)
        {
            var result = await _serviceManager.TechnicianService.UpdateAsync(GetUserId(),dto);

            return HandleResult(result);
        }

        [HttpDelete("Delete{id}")]
        public async Task<ActionResult<bool>> Delete(string id)
        {
            var result = await _serviceManager.TechnicianService.DeleteAsync(id);
            return HandleResult(result);
        }
        [HttpPost("AddDocuments")]
        public async Task<ActionResult<bool>> UploadDocuments( [FromForm] UploadDocumentsDto dto)
        {
            var result = await _serviceManager.TechnicianService.UploadDocumentsAsync(GetUserId(), dto);
            return HandleResult(result);
        }

        [HttpPatch("Availability")]
        public async Task<ActionResult<bool>> ToggleAvailability( [FromQuery] bool isAvailable)
        {
            var result = await _serviceManager.TechnicianService.ToggleAvailabilityStatusAsync(GetUserId(), isAvailable);

            return HandleResult(result);
        }


    }
}
