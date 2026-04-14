using AutoMapper;
using Domain.Contracts;
using Domain.Entities.UsersEntity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens.Experimental;
using Service.Specifications;
using ServiceAbstraction;
using Shared;
using Shared.CommonResult;
using Shared.DTOs.TechnicianDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class TechnicianService(IMapper _mapper, IUnitOfWork _unitOfWork, 
        IFileService _fileService,UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager) : ITechnicianService
    {
        public async Task<Result<IEnumerable<TechnicialDto>>> GetAllAsync(TechnicianQuery query)
        {
            var sp = new TechnicianSpecifications(query);

            var technicians = await _unitOfWork.TechnicalRepository.GetAllAsync(sp);
            if (technicians == null || !technicians.Any())
            {
                return Result<IEnumerable<TechnicialDto>>.Ok(Enumerable.Empty<TechnicialDto>());
            }
            var mappedData = _mapper.Map<IEnumerable<TechnicialDto>>(technicians);

            return Result<IEnumerable<TechnicialDto>>.Ok(mappedData);
        }

        public async Task<Result<TechniciaDetailsDto>> GetByIdAsync(string id)
        {
            var sp=new TechnicianSpecifications(id);
            var technician = await _unitOfWork.TechnicalRepository.GetByIdAsync(sp);

            if (technician == null)
                return Error.NotFound("الفني غير موجود " ,$"الفني {id} غير موجود  ");
               //return Result<TechnicianDetailsDto>.Fail(Error.NotFound("الفني غير موجود ", $"الفني {id} غير موجود  "));
            
             return _mapper.Map<TechniciaDetailsDto>(technician);

            // return Result<TechnicianDetailsDto>.Ok (_mapper.Map<TechnicianDetailsDto>(technician));
        }

        public async Task<Result> AddAsync(string id, AddTechnicianDto technicianDto)
        {
            // 1. Fail-Fast Validations
            
            if (technicianDto == null)
                return Error.Validation("بيانات غير صالحة", "البيانات المرسلة غير صحيحة");

            var user = await userManager.FindByIdAsync(id);
            if (user == null)
                return Error.NotFound("المستخدم غير موجود", $"لا يوجد مستخدم بالمعرف {id}");
            var sp = new TechnicianSpecifications(id);
            var existing = await _unitOfWork.TechnicalRepository.GetByIdAsync(sp);
            if (existing != null)
                return Error.Validation("موجود بالفعل", "هذا المستخدم مسجل بالفعل كفني");
            // 2. Transaction start
            using var transaction = await _unitOfWork.BeginTransactionAsync();
            string? uploadedFilePath = null;
            //(Local Function) 
            async Task RevertChangesAsync()
            {
                await transaction.RollbackAsync();

                if (!string.IsNullOrEmpty(uploadedFilePath))
                {
                    await _fileService.DeleteAsync(uploadedFilePath); // أو DeleteAsync حسب مسمى الدالة عندك
                }
            }
            try
            {
                // 3. saveFile
                var uploadResult = await _fileService.SaveFileAsync(technicianDto.Image, "ProfileImage");
                if (!uploadResult.IsSuccess)
                    return uploadResult; 

                uploadedFilePath = uploadResult.Value;

                // 4-Database Operations
                 //4.1 UpdatUser
                    user.FullName = technicianDto.FullName;
                    user.ProfileImageURL = uploadedFilePath;
                    var updateResult = await userManager.UpdateAsync(user);

                    if (!updateResult.Succeeded)
                    {
                        await RevertChangesAsync();
                        return Error.Failure("تحديث_فشل", "فشل تحديث بيانات المستخدم");
                    }

                

                  // 4.3- Create And Save Tech
                    var technician = _mapper.Map<Technician>(technicianDto);
                    technician.Id = id;

                     await _unitOfWork.TechnicalRepository.AddAsync(technician);
                     await _unitOfWork.SaveAsync();
                // 5 (Commit)
                await transaction.CommitAsync();

                return Result.Ok();
            }
            catch (Exception ex)
            {

                await RevertChangesAsync();

                // _logger.LogError(ex, "System error occurred during Add Technician process.");

                return Error.Failure("خطأ_نظام", "حدث خطأ غير متوقع بالنظام ولم يتم حفظ البيانات.");
            }
        }


        public async Task<Result> UpdateAsync(string id,UpdateTechnicianDto technicianDto)
        {

            if (technicianDto == null)
                return Error.Validation("بيانات غير صالحة", "البيانات المرسلة فارغة");
            var sp = new TechnicianSpecifications(id);
            // get tec 
            var technician = await _unitOfWork.TechnicalRepository.GetByIdAsync(sp);
            if (technician == null)
                return Error.NotFound("الفني غير موجود", $"لا يوجد فني بالمعرف {id}");
            //GET USER update fullname ,image 
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
                   return Error.NotFound("المستخدم غير موجود", $"لا يوجد مستخدم بالمعرف {id}");
            user.FullName=  technicianDto.FullName;
            if (technicianDto.ImageUrl != null && technicianDto.ImageUrl.Length > 0)
            {
                var uploadResult = await _fileService.SaveFileAsync(technicianDto.ImageUrl, "ProfileImage");
                if (!uploadResult.IsSuccess)
                    return uploadResult;
                if(!string.IsNullOrEmpty(user.ProfileImageURL))
                    await _fileService.DeleteAsync(user.ProfileImageURL);
                user.ProfileImageURL = uploadResult.Value;
            }
            var updateResult = await userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
                return Error.Failure(
                    "فشل تحديث المستخدم",
                    string.Join(", ", updateResult.Errors.Select(e => e.Description))
                );
            technician.Bio  = technicianDto.Bio;
            technician.ExperienceYears = technicianDto.ExperienceYears;
            technician.InspectedPrice = technicianDto.InspectedPrice;
            technician.City= technicianDto.City;
            technician.Government= technicianDto.Government;
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
            var sp = new TechnicianSpecifications(id);
            var technician = await _unitOfWork.TechnicalRepository.GetByIdAsync(sp);

            if (technician == null)
                return Error.NotFound(
                    "الفني غير موجود",
                    $"لا يوجد فني بالمعرف {id}"
                );
            var user = await userManager.FindByIdAsync(id);
            await userManager.DeleteAsync(user);

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
            var sp = new TechnicianSpecifications(technicianId);
            // 2. Check technician
            var technician = await _unitOfWork.TechnicalRepository.GetByIdAsync(sp);

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
            var sp = new TechnicianSpecifications(technicianId);
            var technician = await _unitOfWork.TechnicalRepository.GetByIdAsync(sp);

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

        public async Task<Result<bool>> ChangeIsActive(string id, bool state)
        {
            //get client 
            if (id == null)
                return Result<bool>.Fail(Error.Validation("بيانات غير صالحة", "يجب إدخال بيانات الفني"));

            var sp = new TechnicianSpecifications(id);
            var tec = await _unitOfWork.TechnicalRepository.GetByIdAsync(sp);

            if (tec == null)
                return Error.NotFound("الفني غير موجود", $"لا يوجد الفني بالمعرف {id}");
            // is active false 

            tec.IsActive = state;
            // update 
            _unitOfWork.TechnicalRepository.Update(tec);
            //save change
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
            //return Ok.Success();
            return Result<bool>.Ok(true);
        }

        public async Task<Result<int>> CountAsync()
        {
            var count = await _unitOfWork.TechnicalRepository.CountAsync();
            return Result<int>.Ok(count);
        }

        public async Task<Result<GetDocumentDto>> GetDocument(string id)
        {
            var sp = new TechnicianSpecifications(id);
            var technician = await _unitOfWork.TechnicalRepository.GetByIdAsync(sp);
            if (technician == null)
                return Error.NotFound("الفني غير موجود ", $"الفني {id} غير موجود  ");
            
            var doc=_mapper.Map<GetDocumentDto>(technician);
            
            return Result<GetDocumentDto>.Ok(doc);
        }
    }
}