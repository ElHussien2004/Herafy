using Domain.Entities.OrderEntity;
using Microsoft.AspNetCore.Http;
using Shared.CommonResult;
using Shared.DTOs.OrderDtos;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface IOrderService
    {

        Task<Result<IEnumerable<GetAllOrderDTO>>> GetAll();
        Task<Result<int>> CreateOrder(CreateOrderDto createOrderDto);

        Task<Result<IEnumerable<GetClientOrderDTO>>> GetClientOrders(string ClientId,State state);
        Task<Result<IEnumerable<GetTechnicianOrder>>> GetTechnicianOrders(string TecId, State state);


        Task<Result<GetDetailsOrderClientDTO>> GetOrderDetailsClient(int OrderId);
        Task<Result<GetDetailsOrderTechnicianDTO>> GetOrderDetailsTechnician(int OrderId);
        Task<Result<GetDetailsOrderAdminDTO>> GetOrderDetailsAdmin(int OrderId);
        Task<Result> UpdateStatusAsync(int id, int statusValue);
        Task<Result> UpdateFinalPriceAsync(int id, decimal finalPrice);
       
        Task<Result>CompleteOrder (int OrderId,IFormFile? WorkImage);
        Task<Result<int>>CountOrderTechnicianAsync(string TechnicianId);
        Task<Result<int>> CountOrderClientAsync(string clientId);
        Task<Result<int>> CountOrdersCompleted();
    }
}
