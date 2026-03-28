using AutoMapper;
using Domain.Contracts;
using Domain.Entities.UsersEntity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens.Experimental;
using ServiceAbstraction;
using Shared.CommonResult;
using Shared.DTOs.TechnicianDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class TechnicianService(IMapper _mapper, IUnitOfWork _unitOfWork, IFileService _fileService,UserManager<ApplicationUser> userManager) : ITechnicianService
    {
        public async Task<Result<IEnumerable<TechnicianDto>>> GetAllAsync()
        {
            var technicians = await _unitOfWork.TechnicalRepository.GetAllAsync();

            if (technicians == null)
                //return Result<IEnumerable<TechnicianDto>>.Fail(Error.NotFound());
                return Error.NotFound("لا توجد بيانات", "لا يوجد فنيين متاحين حالياً");

            return Result < IEnumerable < TechnicianDto >>.Ok( _mapper.Map<IEnumerable<TechnicianDto>>(technicians));
        }

        public async Task<Result<TechnicialDetailsDto>> GetByIdAsync(string id)
        {
            var technician = await _unitOfWork.TechnicalRepository.GetByIdAsync(id);

            if (technician == null)
                return Error.NotFound("الفني غير موجود " ,$"الفني {id} غير موجود  ");
               //return Result<TechnicianDetailsDto>.Fail(Error.NotFound("الفني غير موجود ", $"الفني {id} غير موجود  "));
            
             return _mapper.Map<TechnicialDetailsDto>(technician);

            // return Result<TechnicianDetailsDto>.Ok (_mapper.Map<TechnicianDetailsDto>(technician));
        }

        public async Task<Result> AddAsync(AddTechnicianDto technicianDto)
        {
            // 1. Validate input
            if (technicianDto == null)
                return Error.Validation("بيانات غير صالحة", "البيانات المرسلة غير صحيحة");

            // 2. Get user
            var user = await userManager.FindByIdAsync(technicianDto.UserId);

            if (user == null)
                return Error.NotFound(
                    "المستخدم غير موجود",
                    $"لا يوجد مستخدم بالمعرف {technicianDto.UserId}"
                );

            // 3. Check if already technician
            var existing = await _unitOfWork.TechnicalRepository
                .GetByIdAsync(technicianDto.UserId);

            if (existing != null)
                return Error.Validation(
                    "موجود بالفعل",
                    "هذا المستخدم مسجل بالفعل كفني"
                );

            // 4. Upload image
            var uploadResult = await _fileService.SaveFileAsync(
                technicianDto.Image,
                "ProfileImage"
            );

            if (!uploadResult.IsSuccess)
                return uploadResult;

            // 5. Update user
            user.FullName = technicianDto.FullName;
            user.ProfileImageURL = uploadResult.Value;

            var updateResult = await userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
                return  Error.Failure(
                    "فشل تحديث المستخدم",
                    "حدث خطأ أثناء تحديث بيانات المستخدم"
                );

            // 6. Create technician
            var technician = _mapper.Map<Technician>(technicianDto);

            await _unitOfWork.TechnicalRepository.AddAsync(technician);

            // 7. Save changes
            await _unitOfWork.SaveAsync();

            return Result.Ok();
        }





        public async Task UpdateAsync(UpdateTechnicianDto technicianDto)
        {
            var technician = await _unitOfWork.TechnicalRepository.GetByIdAsync(technicianDto.Id);

            if (technician == null)
                throw new KeyNotFoundException("Technician not found");

            _mapper.Map(technicianDto, technician);
           _unitOfWork.TechnicalRepository.Update(technician);
            await _unitOfWork.SaveAsync();
        }
        public async Task<bool> DeleteAsync(string id)
        {
            var technician = await _unitOfWork.TechnicalRepository.GetByIdAsync(id);

            if (technician == null)
                return false;

            _unitOfWork.TechnicalRepository.Remove(technician);

            var result = await _unitOfWork.SaveAsync();

            return result > 0;
        }
        public async Task<bool> UploadDocumentsAsync(string technicianId, UploadDocumentsDto documents)
        {
            if (documents == null)
                throw new ArgumentNullException(nameof(documents));

            if (documents.FaceImage == null || documents.BackImage == null)
                throw new ArgumentException("Both images are required");

            var technician = await _unitOfWork.TechnicalRepository.GetByIdAsync(technicianId);

            if (technician == null)
                return false;

            // Upload new files
            /*var facePath = await _fileService.UploadFile(documents.FaceImage);
            var backPath = await _fileService.UploadFile(documents.BackImage);*/

            // Delete old files
            if (technician.Document?.FaceImageUrl != null)
                await _fileService.DeleteFile(technician.Document.FaceImageUrl);

            if (technician.Document?.BackImageUrl != null)
                await _fileService.DeleteFile(technician.Document.BackImageUrl);

            // Create document if not exists
            if (technician.Document == null)
            {
                technician.Document = new TechnicianDocument
                {
                    TechnicianId = technician.Id
                };
            }

           /* technician.Document.FaceImageUrl = facePath;
            technician.Document.BackImageUrl = backPath;*/
            technician.Document.UploadedAt = DateTime.UtcNow;

            await _unitOfWork.SaveAsync();

            return true;
        }
        public async Task<bool> ToggleAvailabilityStatusAsync(string technicianId, bool isAvailable)
        {
            var technician = await _unitOfWork.TechnicalRepository.GetByIdAsync(technicianId);

            if (technician == null)
                return false;

            technician.AvailabilityStatus = isAvailable;

            await _unitOfWork.SaveAsync();

            return true;
        }

    }
}