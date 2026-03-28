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



        public async Task<Result> UpdateAsync(UpdateTechnicianDto technicianDto)
        {

            if (technicianDto == null)
                return Error.Validation("بيانات غير صالحة", "البيانات المرسلة فارغة");
            //GET USER update fullname ,image 
            var user = await userManager.FindByIdAsync(technicianDto.Id);
            if (user == null)
                   return Error.NotFound("المستخدم غير موجود", $"لا يوجد مستخدم بالمعرف {technicianDto.Id}");
            user.FullName=  technicianDto.FullName;
            if (technicianDto.ImageUrl != null && technicianDto.ImageUrl.Length > 0)
            {
                var uploadResult = await _fileService.SaveFileAsync(technicianDto.ImageUrl, "ProfileImage");
                if (!uploadResult.IsSuccess)
                    return uploadResult;
                user.ProfileImageURL = uploadResult.Value;
            }
            var updateResult = await userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
                return Error.Failure(
                    "فشل تحديث المستخدم",
                    string.Join(", ", updateResult.Errors.Select(e => e.Description))
                );

            // get tec 
            var technician = await _unitOfWork.TechnicalRepository.GetByIdAsync(technicianDto.Id);
            if (technician == null)
                return Error.NotFound("الفني غير موجود", $"لا يوجد فني بالمعرف {technicianDto.Id}");

            technician.Bio  = technicianDto.Bio;
            technician.ExperienceYears = technicianDto.ExperienceYears;
            technician.InspectedPrice = technicianDto.InspectedPrice;

            //update tec
            _unitOfWork.TechnicalRepository.Update(technician);
            try
            {
                await _unitOfWork.SaveAsync();
            }
            catch (Exception)
            {
                return Error.Failure(
                    "فشل حفظ البيانات",
                    "حدث خطأ أثناء حفظ التعديلات"
                );
            }
            return  Result.Ok();
           
        }


        public async Task<Result<bool>> DeleteAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Error.Validation(
                    "معرف غير صالح",
                    "يجب إدخال معرف صالح للفني"
                );

            var technician = await _unitOfWork.TechnicalRepository.GetByIdAsync(id);

            if (technician == null)
                return Error.NotFound(
                    "الفني غير موجود",
                    $"لا يوجد فني بالمعرف {id}"
                );

            _unitOfWork.TechnicalRepository.Remove(technician);

            try
            {
                var result = await _unitOfWork.SaveAsync();

                if (result == 0)
                    return Error.Failure(
                        "فشل الحذف",
                        "لم يتم حذف الفني"
                    );

                return true;
            }
            catch (Exception)
            {
                return Error.Failure(
                    "خطأ في قاعدة البيانات",
                    "حدث خطأ أثناء حذف الفني"
                );
            }
        }


        public async Task<Result<bool>> UploadDocumentsAsync(string technicianId, UploadDocumentsDto documents)
        {
            // 1. Validate input
            if (string.IsNullOrWhiteSpace(technicianId) || documents == null)
                return Error.Validation("بيانات غير صالحة", "يجب تقديم معرف الفني والوثائق");

            if (documents.FaceImage == null || documents.BackImage == null)
                return Error.Validation("ملفات غير مكتملة", "يجب رفع صورة الوجه وصورة الخلف");

            // 2. Check technician
            var technician = await _unitOfWork.TechnicalRepository.GetByIdAsync(technicianId);

            if (technician == null)
                return Error.NotFound("الفني غير موجود", $"لا يوجد فني بالمعرف {technicianId}");

            // 3. Upload files
            var saveReFace = await _fileService.SaveFileAsync(documents.FaceImage, "TechnicianDocuments");
            if (!saveReFace.IsSuccess)
                return Error.Failure("فشل عمليه الحفظ", "حدث غطأ في الملفات المرسلة  يرجو الرفع مره اخري");


            var saveReBack = await _fileService.SaveFileAsync(documents.BackImage, "TechnicianDocuments");
            if (!saveReBack.IsSuccess)
                 return Error.Failure("فشل عمليه الحفظ", "حدث غطأ في الملفات المرسلة  يرجو الرفع مره اخري");
            ;

            // 4. Create document
            var document = new TechnicianDocument
            {
                FaceImageUrl = saveReFace.Value,
                BackImageUrl = saveReBack.Value,
                TechnicianId = technicianId
            };

            await _unitOfWork.DocumentRepository.AddAsync(document);

            // 5. Save changes
            try
            {
                await _unitOfWork.SaveAsync();
            }
            catch
            {
                return Error.Failure("فشل حفظ البيانات", "حدث خطأ أثناء حفظ الوثائق");
            }

            return true;
        }


        public async Task<Result<bool>> ToggleAvailabilityStatusAsync(string technicianId, bool isAvailable)
        {
            if (string.IsNullOrWhiteSpace(technicianId))
                return Error.Validation(
                    "معرف غير صالح",
                    "يجب إدخال معرف صالح للفني"
                );

            var technician = await _unitOfWork.TechnicalRepository.GetByIdAsync(technicianId);

            if (technician == null)
                return Error.NotFound(
                    "الفني غير موجود",
                    $"لا يوجد فني بالمعرف {technicianId}"
                );

            if (technician.AvailabilityStatus == isAvailable)
                return Error.Validation(
                    "لا يوجد تغيير",
                    "حالة التوفر بالفعل كما هي"
                );

            technician.AvailabilityStatus = isAvailable;

            try
            {
                var result = await _unitOfWork.SaveAsync();

                if (result == 0)
                    return Error.Failure(
                        "فشل التحديث",
                        "لم يتم تحديث حالة التوفر"
                    );
            }
            catch
            {
                return Error.Failure(
                    "خطأ في قاعدة البيانات",
                    "حدث خطأ أثناء تحديث حالة التوفر"
                );
            }

            return true;
        }

    }
}