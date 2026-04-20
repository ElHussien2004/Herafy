using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared;
using Shared.DTOs.ClientDTOS;
using Shared.DTOs.TechnicianDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class ClientController(IServiceManager _serviceManager):ApiBaseController
    {
        [Authorize(Roles =Roles.Client)]
        [HttpPost("AddClient")]
        public async Task<IActionResult> Add([FromForm] AddClientDto dto)
        {
            var result = await _serviceManager.ClientService.AddAsync(GetUserId(),dto);

            return HandleResult(result);
        }
        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("Delete{id}")]
        public async Task<ActionResult<bool>> Delete(string id)
        {
            var result = await _serviceManager.ClientService.DeleteAsync(id);

            return HandleResult(result);
        }
        [Authorize(Roles = Roles.Client)]
        [HttpPatch("Update")]
        public async Task<IActionResult> Update([FromForm] UpdataClientdto dto)
        {
            var result = await _serviceManager.ClientService.UpdateAsync(GetUserId(), dto);

            return HandleResult(result);
        }
        [Authorize(Roles = Roles.Client)]
        [HttpGet("Profile")]
        public async Task<ActionResult<ClientDetailsDto>> GetProfile()
        {
            var result = await _serviceManager.ClientService.GetByIdAsync(GetUserId());
            return HandleResult(result);
        }
        [Authorize(Roles = Roles.Client)]
        [HttpGet("TechnicianDetails{id}")] //الصفحه دي بجيبها العميل  
        public async Task<ActionResult<TechniciaDetailsDto>> GetTec(string id)
        {
            var result = await _serviceManager.TechnicianService.GetByIdAsync(id);
            return HandleResult(result);
        }

    }
}
