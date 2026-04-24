using Domain.Entities.OrderEntity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared;
using Shared.DTOs.OrderDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class OrderController(IServiceManager _serviceManager) : ApiBaseController
    {
       //-1
        [Authorize(Roles = Roles.Client)]
        [HttpPost("CreateOrder")]
        public async Task<ActionResult<int>> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            var result = await _serviceManager.OrderService.CreateOrder(createOrderDto);
            return HandleResult(result);
        }
        //-2
        [Authorize(Roles = Roles.Client)]
        [HttpGet("GetDetailsOrderClient/{orderId:int}")]
        public async Task<ActionResult<GetDetailsOrderClientDTO>> GetOrderDetailsForClient(int orderId)
        {
            var result = await _serviceManager.OrderService.GetOrderDetailsClient(orderId);
            return HandleResult(result);
        }
        //-5
        [Authorize(Roles = Roles.Technician)]
        [HttpGet("GetDetailsOrderTechnician/{orderId:int}")]
        public async Task<ActionResult<GetDetailsOrderTechnicianDTO>> GetOrderDetailsForTechnician(int orderId)
        {
            var result = await _serviceManager.OrderService.GetOrderDetailsTechnician(orderId);
            return HandleResult(result);
        }
        //-9
        [Authorize(Roles = Roles.Admin)]
        [HttpGet("GetDetailsOrderAdmin/{orderId:int}")]
        public async Task<ActionResult<GetDetailsOrderAdminDTO>> GetOrderDetailsForAdmin(int orderId)
        {
            var result = await _serviceManager.OrderService.GetOrderDetailsAdmin(orderId);
            return HandleResult(result);
        }
        //-11
        [Authorize(Roles = Roles.Technician + "," + Roles.Client)]
        [HttpPatch("{orderId:int}/ChangeStatusOrder")]
        public async Task<IActionResult> UpdateStatus(int orderId, [FromBody] int statusValue)
        {
            var result = await _serviceManager.OrderService.UpdateStatusAsync(orderId, statusValue);
            return HandleResult(result);
        }
        //-10
        [Authorize(Roles = Roles.Technician)]
        [HttpPatch("{orderId:int}/SetFinalPrice")]
        public async Task<IActionResult> UpdatePrice(int orderId, [FromBody] decimal finalPrice)
        {
            var result = await _serviceManager.OrderService.UpdateFinalPriceAsync(orderId, finalPrice);
            return HandleResult(result);
        }
        //-6
        [Authorize(Roles = Roles.Technician)]
        [HttpGet("{orderId:int}/CompleteOrder")]
        public async Task<IActionResult> CompleteOrder(int orderId,[FromBody]IFormFile WorkImage)
        {
            var result = await _serviceManager.OrderService.CompleteOrder(orderId, WorkImage);
            return HandleResult(result);
        }
        //-8
        [Authorize(Roles = Roles.Admin)]
        [HttpGet("GetAllOrder")]
        public async Task<ActionResult<IEnumerable<GetAllOrderDTO>>> GetAllOrders()
        {
            var result = await _serviceManager.OrderService.GetAll();
            return HandleResult(result);
        }
        //3
        [Authorize(Roles = Roles.Client)]
        [HttpGet("GetClientOrders/{clientId}/{State}")]
        public async Task<ActionResult<IEnumerable<GetClientOrderDTO>>> GetClientOrders([FromQuery] string clientId, [FromQuery] State state)
        {
            var result = await _serviceManager.OrderService.GetClientOrders(clientId, state);
            return HandleResult(result);
        }
        //-4
        [Authorize(Roles = Roles.Technician)]
        [HttpGet("GetTechnicianOrders/{techId}/{State}\")")]
        public async Task<ActionResult<IEnumerable<GetTechnicianOrder>>> GetTechnicianOrders([FromQuery] string techId, [FromQuery] State state)
        {
            var result = await _serviceManager.OrderService.GetTechnicianOrders(techId, state);
            return HandleResult(result);
        }
        //-7
        [Authorize(Roles = Roles.Admin)]
        [HttpGet("CountCompletedOrders")]
        public async Task<ActionResult<int>> GetCompletedCount()
        {
            var result = await _serviceManager.OrderService.CountOrdersCompleted();
            return HandleResult(result);
        }
    }
}
