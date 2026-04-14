using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.DTOs.ClientDTOS;
using Shared.DTOs.TechnicianDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Authorize(Roles = "Client")]
    public class ClientController(IServiceManager _serviceManager):ApiBaseController
    {
        [HttpPost("AddClient")]
        public async Task<IActionResult> Add([FromForm] AddClientDto dto)
        {
            var result = await _serviceManager.ClientService.AddAsync(GetUserId(),dto);

            return HandleResult(result);
        }
        [HttpDelete("Delete{id}")]
        public async Task<ActionResult<bool>> Delete(string id)
        {
            var result = await _serviceManager.ClientService.DeleteAsync(id);

            return HandleResult(result);
        }
        [HttpPatch("Update")]
        public async Task<IActionResult> Update([FromForm] UpdataClientdto dto)
        {
            var result = await _serviceManager.ClientService.UpdateAsync(GetUserId(), dto);

            return HandleResult(result);
        }
        [HttpGet("Profile")]
        public async Task<ActionResult<ClientDto>> GetProfile()
        {
            var result = await _serviceManager.ClientService.GetByIdAsync(GetUserId());
            return HandleResult(result);
        }
    }
}
