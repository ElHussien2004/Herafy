using AutoMapper;
using Domain.Contracts;
using Domain.Entities.OrderEntity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Service.Specifications;
using ServiceAbstraction;
using Shared;
using Shared.CommonResult;
using Shared.DTOs.OrderDtos;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Order = Domain.Entities.OrderEntity.Order;

namespace Service
{
    public class OrderService(IUnitOfWork _unitOfWork ,IMapper _mapper, ILogger<OrderService> _logger,IFileService _fileService) : IOrderService
    {
        public async Task<Result<int>> CreateOrder(CreateOrderDto createOrderDto)
        {
            if (createOrderDto == null)
                return Error.Validation("بيانات غير صالحة", "البيانات المرسلة غير صحيحة");

            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                // 3. Database Operations

                // 3.1 Mapping DTO to Entity
                var order = _mapper.Map<Order>(createOrderDto);

                // 3.2 Setting Default Values
                order.Status = State.Pending; // الحالة الافتراضية "قيد المراجعة"

                // 3.3 Save to Repository
                await _unitOfWork.OrderRepo.AddAsync(order);
                await _unitOfWork.SaveAsync();

                // 4. Commit
                await transaction.CommitAsync();

                return Result<int>.Ok(order.Id);
            }
            catch (Exception ex)
            {
                // 5. Rollback on failure
                await transaction.RollbackAsync();

                _logger.LogError(ex, "Error occurred while creating order for client {ClientId}", createOrderDto.ClientId);
                return Error.Failure("خطأ_نظام", "حدث خطأ أثناء إنشاء الطلب، حاول مرة أخرى.");
            }

        }
        public async Task<Result<GetDetailsOrderClientDTO>> GetOrderDetailsClient(int OrderId)
        {
            if (OrderId <= 0)
                return Error.Validation("معرف غير صالح", "رقم الطلب يجب أن يكون أكبر من صفر");
            try
            {
                // 2. Prepare Specification with Includes
                var OrderSpac = new OrderQuery()
                {
                    OrderId= OrderId
                };
                var spec = new OrderWithDetailsSpecification(OrderSpac);

                // 3. Fetch from Repository
                var order = await _unitOfWork.OrderRepo.GetByIdAsync(spec);

                if (order == null)
                    return Error.NotFound("الطلب غير موجود", $"لا يوجد طلب مسجل بالرقم {OrderId}");

                // 4. Mapping to DTO
                var dto = _mapper.Map<GetDetailsOrderClientDTO>(order);

                // 5. Return Success Result
                return Result<GetDetailsOrderClientDTO>.Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching order details for client. Order ID: {OrderId}", OrderId);
                return Error.Failure("خطأ_نظام", "حدث خطأ أثناء تحميل تفاصيل الطلب.");
            }
        }

        public async Task<Result<GetDetailsOrderTechnicianDTO>> GetOrderDetailsTechnician(int OrderId)
        {
            // 1. Fail-Fast Validation
            if (OrderId <= 0)
                return Error.Validation("معرف غير صالح", "رقم الطلب غير صحيح.");

            try
            {
                // 2. Prepare Specification 
                var OrderSpac = new OrderQuery()
                {
                    OrderId = OrderId
                };
                var spec = new OrderWithDetailsSpecification(OrderSpac);

                // 3. Fetch from Repository
                var order = await _unitOfWork.OrderRepo.GetByIdAsync(spec);

                if (order == null)
                    return Error.NotFound("المهمة غير موجودة", "لا يمكن العثور على تفاصيل هذه المهمة.");

                // 4. Mapping to DTO
                var dto = _mapper.Map<GetDetailsOrderTechnicianDTO>(order);

                // 5. Return Success
                return Result<GetDetailsOrderTechnicianDTO>.Ok(dto);
            }
            catch (Exception ex)
            {
                 _logger.LogError(ex, "Error fetching mission details for technician");
                return Error.Failure("خطأ_نظام", "حدث خطأ أثناء تحميل بيانات المهمة.");
            }
        }

        public async Task<Result> UpdateStatusAsync(int OrderId, int statusValue)
        {
            // 1. Fail-Fast Validations
            if (OrderId <= 0)
                return Error.Validation("معرف غير صالح", "رقم الطلب غير صحيح.");

            
            if (!Enum.IsDefined(typeof(State), statusValue))
                return Error.Validation("حالة غير صالحة", "قيمة الحالة المرسلة غير معرفة بالنظام.");

            try
            {
                // 2. Fetch Order
                var OrderSpac = new OrderQuery()
                {
                    OrderId = OrderId
                };
                var spec = new OrderWithDetailsSpecification(OrderSpac);
                var order = await _unitOfWork.OrderRepo.GetByIdAsync(spec);

                if (order == null)
                    return Error.NotFound("الطلب غير موجود", "لا يمكن العثور على الطلب لتحديث حالته.");

                if (order.Status == State.Completed || order.Status == State.Rejected)
                    return Error.Validation("تحديث مرفوض", "لا يمكن تغيير حالة طلب منتهي أو ملغي.");

                // 4. Database Operations
                order.Status = (State)statusValue; // Casting الـ int لـ Enum

                _unitOfWork.OrderRepo.Update(order);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                    return Result.Ok();

                return Error.Failure("فشل التحديث", "حدثت مشكلة أثناء حفظ الحالة الجديدة.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating status for order {OrderId}", OrderId);
                return Error.Failure("خطأ_نظام", "حدث خطأ غير متوقع أثناء تحديث حالة الطلب.");
            }
        }

        public async Task<Result> UpdateFinalPriceAsync(int OrderId, decimal finalPrice)
        {
            // 1. Fail-Fast Validations
            if (OrderId <= 0)
                return Error.Validation("معرف غير صالح", "رقم الطلب غير صحيح.");

            if (finalPrice <= 0)
                return Error.Validation("سعر غير صالح", "يجب إدخال قيمة صحيحة للسعر النهائي.");

            try
            {
                // 2. Fetch Order
                var OrderSpac = new OrderQuery()
                {
                    OrderId = OrderId
                };
                var spec = new OrderWithDetailsSpecification(OrderSpac);
                var order = await _unitOfWork.OrderRepo.GetByIdAsync(spec);

                if (order == null)
                    return Error.NotFound("الطلب غير موجود", "لا يمكن العثور على الطلب لتحديث سعره.");

                // 3. Database Operations
                order.FinalPrice = finalPrice;

                _unitOfWork.OrderRepo.Update(order);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                    return Result.Ok();

                return Error.Failure("فشل التحديث", "حدثت مشكلة أثناء حفظ السعر الجديد.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching Update Final Price ");
                return Error.Failure("خطأ_نظام", "حدث خطأ غير متوقع أثناء تحديث الفاتورة.");
            }
        }

        public async Task<Result<int>> CountOrdersCompleted()
        {
            var OrderSpac = new OrderQuery()
            {
                State=State.Completed
            };
            var spec = new OrderWithDetailsSpecification(OrderSpac);

            var count = await _unitOfWork.OrderRepo.CountAsync(spec);

            return Result<int>.Ok(count);
        }
        public async Task<Result> CompleteOrder(int OrderId,IFormFile? WorkImage)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var spec = new OrderWithDetailsSpecification(new OrderQuery { OrderId = OrderId });
                var order = await _unitOfWork.OrderRepo.GetByIdAsync(spec);

                if (order == null)
                    return Error.NotFound("الطلب غير موجود", "لا يمكن العثور على الطلب.");

                if (order.Status == State.Completed)
                    return Error.Validation("تنبيه", "الطلب مكتمل بالفعل.");

                
                var techSpec = new TechnicianSpecifications(order.TechnicianId);
                var technician = await _unitOfWork.TechnicalRepository.GetByIdAsync(techSpec);

                if (technician != null)
                {
                    technician.CompletedJobs += 1;
                    _unitOfWork.TechnicalRepository.Update(technician);
                }

                order.Status = State.Completed;
              
               if(WorkImage ==null)
               {
                    _unitOfWork.OrderRepo.Update(order);
               }
               else
               {
                    var res = await _fileService.SaveFileAsync(WorkImage, "WorkImages");
                    if (res.IsSuccess)
                    {
                        order.ImageWorkURL = res.Value;
                    }
                    _unitOfWork.OrderRepo.Update(order);
                }
                

                await _unitOfWork.SaveAsync();
                await transaction.CommitAsync();
                _logger.LogInformation("Order {OrderId} marked as COMPLETED. Technician: {TechId}", OrderId, order.TechnicianId);

                return Result.Ok();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Failed to complete order {OrderId}", OrderId);
                return Error.Failure("خطأ_نظام", "حدث خطأ أثناء إنهاء الطلب.");
            }
        }

        public async Task<Result<int>> CountOrderTechnicianAsync(string Tecid)
        {
            
            var OrderSpac = new OrderQuery()
            {
              
               TechnicianId=Tecid
            };
            var spec = new OrderWithDetailsSpecification(OrderSpac);
            var count = await _unitOfWork.OrderRepo.CountAsync(spec);
            return Result<int>.Ok(count);
        }
        public async Task<Result<int>> CountOrderClientAsync(string ClientId)
        {
            var OrderSpac = new OrderQuery()
            {
                ClintId = ClientId
            };
            var spec = new OrderWithDetailsSpecification(OrderSpac);
            var count = await _unitOfWork.OrderRepo.CountAsync(spec);
            return Result<int>.Ok(count);
        }
        public async Task<Result<IEnumerable<GetAllOrderDTO>>> GetAll()
        {
            try
            {
                var spec = new OrderWithDetailsSpecification();
                var orders = await _unitOfWork.OrderRepo.GetAllAsync(spec);

                var dtos = _mapper.Map<IEnumerable<GetAllOrderDTO>>(orders);
                return Result<IEnumerable<GetAllOrderDTO>>.Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching Get All Orders");
                return Error.Failure("خطأ_نظام", "حدث خطأ أثناء جلب قائمة الطلبات.");
            }
        }

        public async Task<Result<GetDetailsOrderAdminDTO>> GetOrderDetailsAdmin(int OrderId)
        {
            if (OrderId <= 0) return Error.Validation("معرف غير صالح", "رقم الطلب غير صحيح.");

            try
            {
                var OrderSpac = new OrderQuery()
                {
                    OrderId = OrderId
                };
                var spec = new OrderWithDetailsSpecification(OrderSpac); 
                var order = await _unitOfWork.OrderRepo.GetByIdAsync(spec);

                if (order == null) return Error.NotFound("الطلب غير موجود", "لا يمكن العثور على تفاصيل الطلب.");

                var dto = _mapper.Map<GetDetailsOrderAdminDTO>(order);
                return Result<GetDetailsOrderAdminDTO>.Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching mission details for Admin");
                return Error.Failure("خطأ_نظام", "حدث خطأ أثناء تحميل بيانات الأدمن.");
            }
        }


        public async Task<Result<IEnumerable<GetClientOrderDTO>>> GetClientOrders(string ClientId,State state)
        {
            if (string.IsNullOrEmpty(ClientId)) return Error.Validation("بيانات ناقصة", "معرف العميل مطلوب.");
            try
            {
                var OrderSpac = new OrderQuery()
                {
                    ClintId = ClientId
                };
                if (state!=State.Pending)
                {
                   OrderSpac.State = state;
                }
                

                var spec = new OrderWithDetailsSpecification(OrderSpac);
                var orders = await _unitOfWork.OrderRepo.GetAllAsync(spec);

                var dtos = _mapper.Map<IEnumerable<GetClientOrderDTO>>(orders);
                return Result<IEnumerable<GetClientOrderDTO>>.Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching orders for Client {ClientId}", ClientId);
                return Error.Failure("خطأ_نظام", "حدث خطأ أثناء جلب طلبات العميل.");
            }
        }

        public async Task<Result<IEnumerable<GetTechnicianOrder>>> GetTechnicianOrders(string TecId, State state)
        {
            if (string.IsNullOrEmpty(TecId)) return Error.Validation("بيانات ناقصة", "معرف الفني مطلوب.");

            try
            {
                var OrderSpac = new OrderQuery()
                {
                    TechnicianId = TecId,
                    State = state
                };

                var spec = new OrderWithDetailsSpecification(OrderSpac);
                var orders = await _unitOfWork.OrderRepo.GetAllAsync(spec);

                var dtos = _mapper.Map<IEnumerable<GetTechnicianOrder>>(orders);
                return Result<IEnumerable<GetTechnicianOrder>>.Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching orders for technician {TecId}", TecId);
                return Error.Failure("خطأ_نظام", "حدث خطأ أثناء جلب طلبات الفني.");
            }
        }
    }
}
