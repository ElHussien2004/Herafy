using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared;
using Shared.CommonResult;
using Shared.DTOs.ClientDTOS;
using Shared.DTOs.TechnicianDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class AdminController(IServiceManager _serviceManager):ApiBaseController
    {
        [HttpGet("GetAllTechnicians")]
        public async Task<ActionResult<IEnumerable<TechnicialDto>>> GetAll([FromQuery] TechnicianQuery query)
        {
            var result = await _serviceManager.TechnicianService.GetAllAsync(query);

            return HandleResult<IEnumerable<TechnicialDto>>(result);
        }

        [HttpGet("GetAllClient")]
        public async Task<ActionResult<IEnumerable<ClientDto>>> GetAll()
        {
            var result = await _serviceManager.ClientService.GetAllAsync();

            return HandleResult(result);
        }

        [HttpPatch("{id}/ActiveTechnician")]
        public async Task<ActionResult<bool>> ChangeActiveTec(string id, [FromQuery] bool state)
        {
            var result = await _serviceManager.TechnicianService.ChangeIsActive(id, state);

            return HandleResult(result);
        }

        [HttpPatch("{id}/ActiveClient")]
        public async Task<ActionResult<bool>> ChangeActiveCl(string id, [FromQuery] bool state)
        {
            var result = await _serviceManager.ClientService.ChangeIsActive(id, state);

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
        [HttpGet("GetTechnician")]
        public async Task<ActionResult<GetDocumentDto>>GetDocument()
        {
            var result =await _serviceManager.TechnicianService.GetDocument(GetUserId());
            return HandleResult<GetDocumentDto>(result);
        }
    }
}
