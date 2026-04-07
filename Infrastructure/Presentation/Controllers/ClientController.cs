using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.DTOs.ClientDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
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

    }
}
