using AutoMapper;
using Domain.Contracts;
using Domain.Entities.OrderEntity;
using Domain.Entities.UsersEntity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Service.Specifications;
using ServiceAbstraction;
using Shared.CommonResult;
using Shared.DTOs.ComplaintDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ComplaintService(
             IUnitOfWork _unitOfWork, IMapper _mapper,
             UserManager<ApplicationUser> _userManager,
             IHttpContextAccessor _httpContextAccessor) : IComplaintService
    {
        public async Task<Result> CreateComplaintAsync(CreateComplaintDto dto)
        {
            var userId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);

            if (string.IsNullOrEmpty(userId))
                return Error.Unauthorized(description: "يجب تسجيل الدخول أولاً");

            var complaint = _mapper.Map<Complaint>(dto);

            complaint.UserId = userId;
            complaint.CreatedAt = DateTime.UtcNow;
            complaint.Status = ComplaintStatus.Submitted;

            await _unitOfWork.ComplaintRepo.AddAsync(complaint);
            await _unitOfWork.SaveAsync();

            return Result.Ok();
        }

        public async Task<Result<List<GetAllComplaintDto>>> GetAllComplaintsAsync()
        {
            var spec = new ComplaintSpecifications();

            var complaints = await _unitOfWork.ComplaintRepo.GetAllAsync(spec);

            var result = _mapper.Map<List<GetAllComplaintDto>>(complaints);

            // إضافة الدور
            foreach (var dto in result)
            {
                var user = await _userManager.FindByIdAsync(dto.UserId);

                if (user != null)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    dto.UserRole = roles.FirstOrDefault();
                }
            }

            return Result<List<GetAllComplaintDto>>.Ok(result);
        }

        public async Task<Result<GetDetailsComplaintDto>> GetComplaintByIdAsync(int id)
        {
            var spec = new ComplaintSpecifications(id);

            var complaint = await _unitOfWork.ComplaintRepo.GetByIdAsync(spec);

            if (complaint == null)
                return Error.NotFound("Complaint.NotFound", "الشكوى غير موجودة");

            var result = _mapper.Map<GetDetailsComplaintDto>(complaint);

            var user = await _userManager.FindByIdAsync(complaint.UserId);

            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                result.UserRole = roles.FirstOrDefault();
            }

            return Result<GetDetailsComplaintDto>.Ok(result);
        }

        public async Task<Result<ComplaintResponseDto>> RespondToComplaintAsync(int complaintId, string response)
        {
            var spec = new ComplaintSpecifications(complaintId);

            var complaint = await _unitOfWork.ComplaintRepo.GetByIdAsync(spec);

            if (complaint == null)
                return Error.NotFound("Complaint.NotFound", "الشكوى غير موجودة");

            complaint.Response = response;
            complaint.Status = ComplaintStatus.Resolved;

            _unitOfWork.ComplaintRepo.Update(complaint);
            await _unitOfWork.SaveAsync();

            var result = _mapper.Map<ComplaintResponseDto>(complaint);

            return Result<ComplaintResponseDto>.Ok(result);
        }

        public async Task<Result<bool>> DeleteComplaintAsync(int id)
        {
            var spec = new ComplaintSpecifications(id);

            var complaint = await _unitOfWork.ComplaintRepo.GetByIdAsync(spec);

            if (complaint == null)
                return Error.NotFound("Complaint.NotFound", "الشكوى غير موجودة");

            _unitOfWork.ComplaintRepo.Remove(complaint);

            await _unitOfWork.SaveAsync();

            return Result<bool>.Ok(true);
        }
    }
}
