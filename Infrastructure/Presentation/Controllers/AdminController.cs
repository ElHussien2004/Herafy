using Domain.Entities.UsersEntity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared;
using Shared.CommonResult;
using Shared.DTOs.ClientDTOS;
using Shared.DTOs.TechnicianDTOS;
using Shared.DTOs.UserDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class AdminController(IServiceManager _serviceManager):ApiBaseController
    {
        [HttpGet("GetAllTechnicians")]
        public async Task<ActionResult<IEnumerable<TechnicianDto>>> GetAll()
        {
            var result = await _serviceManager.TechnicianService.GetAllAdminAsync();

            return HandleResult<IEnumerable<TechnicianDto>>(result);
        }
        [HttpGet("GetAllClient")]
        public async Task<ActionResult<IEnumerable<ClientDto>>> GetAllClient( )
        {
            var result = await _serviceManager.ClientService.GetAllAsync();

            return HandleResult(result);
        }

        [HttpPatch("ChangeStateTechnician")]
        public async Task<ActionResult<bool>> ChangeActiveTec([FromQuery]string id, [FromQuery] StateUser state)
        {
            var result = await _serviceManager.TechnicianService.ChangeIsActive(id, state);

            return HandleResult(result);
        }

        [HttpPatch("ChangeStateClient")]
        public async Task<ActionResult<bool>> ChangeActiveCl([FromQuery]string id, [FromQuery] StateUser state)
        {
            var result = await _serviceManager.ClientService.ChangeIsActive(id, state);

            return HandleResult(result);
        }
        [HttpDelete("DeleteClient")]
        public async Task<ActionResult<bool>> DeleteClient([FromQuery]string id)
        {
            var result = await _serviceManager.ClientService.DeleteAsync(id);

            return HandleResult(result);
        }

        [HttpDelete("DeleteTechnician")]
        public async Task<ActionResult<bool>> DeleteTechnician([FromQuery] string id)
        {
            var result = await _serviceManager.TechnicianService.DeleteAsync(id);

            return HandleResult(result);
        }
        [HttpGet("CountClients")]
        public async Task<ActionResult<int>>CountClinets()
        {
            var result = await _serviceManager.ClientService.CountAsync();
            
            return HandleResult<int>(result);
        }
        [HttpGet("CountTechnicians")]
        public async Task<ActionResult<int>> CountTech()
        {
            var result = await _serviceManager.TechnicianService.CountAsync();

            return HandleResult<int>(result);
        }
        [HttpGet("GetClientDetails")]
        public async Task<ActionResult<ClientDetailsDto>> GetClientDetails([FromQuery] string id)
        {
            var result = await _serviceManager.ClientService.GetByIdAsync(id);
            return HandleResult<ClientDetailsDto>(result);
        }

        [HttpGet("GetDecumentTechnician")]
        public async Task<ActionResult<GetDocumentDto>>GetDocument([FromQuery] string id)
        {
            var result =await _serviceManager.TechnicianService.GetDocument(id);
            return HandleResult<GetDocumentDto>(result);
        }

        [HttpGet("GetDecumentClient")]
        public async Task<ActionResult<GetDecumentClient>> GetDocumentClient([FromQuery] string id)
        {
            var result = await _serviceManager.ClientService.GetDocument(id);
            return HandleResult<GetDecumentClient>(result);
        }

        [HttpPost ("RejectState")]
        public async Task<IActionResult>RejectState(SendMessageRejectToUserDto toUserDto)
        {
            var result = await _serviceManager.SMSService.SendAsync(toUserDto.PhoneNumber, toUserDto.Messsage);
            return HandleResult(result);
        }

    }
}
