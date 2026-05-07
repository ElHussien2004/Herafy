using AutoMapper;
using Domain.Contracts;
using Domain.Entities.UsersEntity;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;
using Service.Specifications;
using ServiceAbstraction;
using Shared.CommonResult;
using Shared.DTOs.ClientDTOS;
using Shared.DTOs.TechnicianDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ClientService(UserManager<ApplicationUser> _userManager ,RoleManager<IdentityRole> _roleManager,
        IMapper _mapper,IUnitOfWork _unitOfWork ,IFileService _fileService) : IClientService
    {
        public async Task<Result> AddAsync(string userId, AddClientDto clientDto)
        {
            // 1. Fail-Fast Validations
  
            if (clientDto == null)
                return Error.Validation("بيانات غير صالحة", "يجب إدخال بيانات العميل");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return Error.NotFound("المستخدم غير موجود", $"لم يتم العثور على مستخدم بالمعرف {userId}");

            var sp = new ClientSpecifications(userId);
            var existing = await _unitOfWork.ClientRepository.GetByIdAsync(sp);
            if (existing != null)
                return Error.Validation("موجود بالفعل", "هذا المستخدم مسجل بالفعل كعميل");

      
            using var transaction = await _unitOfWork.BeginTransactionAsync();
            string? uploadedFilePath = null;

            async Task RevertChangesAsync()
            {
                await transaction.RollbackAsync();

                if (!string.IsNullOrEmpty(uploadedFilePath))
                {
                    await _fileService.DeleteAsync(uploadedFilePath);
                }
            }

            try
            {
               
                if (clientDto.ProfileImageURL != null && clientDto.ProfileImageURL.Length > 0)
                {
                
                    var uploadResult = await _fileService.SaveFileAsync(clientDto.ProfileImageURL, "ProfileImage");

                    if (!uploadResult.IsSuccess)
                        return uploadResult;

                    uploadedFilePath = uploadResult.Value;
                }

                user.FullName = clientDto.FullName;
                if (uploadedFilePath != null)
                {
                    user.ProfileImageURL = uploadedFilePath;
                }

                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    await RevertChangesAsync();
                    return Error.Failure("فشل_تحديث", "حدث خطأ أثناء تحديث بيانات المستخدم");
                }

                var clientEntity = _mapper.Map<Client>(clientDto);
                clientEntity.Id = userId;

                await _unitOfWork.ClientRepository.AddAsync(clientEntity);

                 await _userManager.AddToRoleAsync(user, "Client");

                await _unitOfWork.SaveAsync();

                await transaction.CommitAsync();

                return Result.Ok();
            }
            catch 
            {
                await RevertChangesAsync();

                // _logger.LogError(ex, "Error while adding a new client.");

                return Error.Failure("فشل_النظام", "حدث خطأ أثناء حفظ التعديلات ولم يتم الحفظ.");
            }
        }

       

        public async Task<Result<int>> CountAsync()
        {
            var count = await _unitOfWork.ClientRepository.CountAsync();
            return Result<int>.Ok(count);
        }

        public async Task<Result<bool>> DeleteAsync(string id)
        {
            //validate id 
            if (id == null)
                return Result<bool>.Fail(Error.Validation("بيانات غير صالحة", "يجب إدخال بيانات العميل"));

            // get client 
            var sp = new ClientSpecifications(id);
            var client = await _unitOfWork.ClientRepository.GetByIdAsync(sp);
            if (client == null)
                return Error.NotFound("العميل غير موجود", $"لا يوجد العميل بالمعرف {id}");
            //get user 
            _unitOfWork.ClientRepository.Remove(client);
            var user =await  _userManager.FindByIdAsync(id);
            await  _userManager.DeleteAsync(user);

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

        public async Task<Result<IEnumerable<ClientDto>>> GetAllAsync()
        {
            
            var sp = new ClientSpecifications();
            var clients= await _unitOfWork.ClientRepository.GetAllAsync(sp);

            if (clients == null || !clients.Any())
            {
                return Result<IEnumerable<ClientDto>>.Ok(Enumerable.Empty<ClientDto>());
            }
            var mappedData = _mapper.Map<IEnumerable<ClientDto>>(clients);

            return Result<IEnumerable<ClientDto>>.Ok(mappedData);
        }

        public async Task<Result<ClientDetailsDto>> GetByIdAsync(string id)
        {
            var sp = new ClientSpecifications(id);
            var client = await _unitOfWork.ClientRepository.GetByIdAsync(sp);
            if (client == null)
                return Error.NotFound("العميل غير موجود ", $"العميل {id} غير موجود  ");

            return Result<ClientDetailsDto>.Ok(_mapper.Map<ClientDetailsDto>(client));
        }

        public async Task<Result> UpdateAsync(string id, UpdataClientdto dto)
        {
            if (dto == null)
                return Error.Validation("بيانات غير صالحة", "البيانات المرسلة فارغة");
            //GET USER update fullname ,image 
            var sp = new ClientSpecifications(id);
            var client = await _unitOfWork.ClientRepository.GetByIdAsync(sp);
            if (client == null)
                return Error.NotFound("عميل غير موجود", $"لا يوجد عميل بالمعرف {id}");
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return Error.NotFound("المستخدم غير موجود", $"لا يوجد مستخدم بالمعرف {id}");
            user.FullName = dto.FullName;
            if (dto.ImageUrl != null && dto.ImageUrl.Length > 0)
            {
                var uploadResult = await _fileService.SaveFileAsync(dto.ImageUrl, "ProfileImage");
                if (!uploadResult.IsSuccess)
                    return uploadResult;
                if (!string.IsNullOrEmpty(user.ProfileImageURL))
                    await _fileService.DeleteAsync(user.ProfileImageURL);
                user.ProfileImageURL = uploadResult.Value;
            }
            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
                return Error.Failure(
                    "فشل تحديث المستخدم",
                    string.Join(", ", updateResult.Errors.Select(e => e.Description))
                );
            client.City = dto.City;
            client.Government = dto.Government;
            //update 
            _unitOfWork.ClientRepository.Update(client);
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
            return Result.Ok();
        }
        public async Task<Result<bool>> ChangeIsActive(string id, StateUser stateUser)
        {
            //get client 
            if (id == null)
                return Result<bool>.Fail(Error.Validation("بيانات غير صالحة", "يجب إدخال بيانات العميل"));

            var sp = new ClientSpecifications(id);
            var client = await _unitOfWork.ClientRepository.GetByIdAsync(sp);

            if (client == null)
                return Error.NotFound("العميل غير موجود", $"لا يوجد العميل بالمعرف {id}");
            // is active false 

            client.State = stateUser;
            // update 
            _unitOfWork.ClientRepository.Update(client);
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

       
        public async  Task<Result<bool>> UploadDocumentsAsync(string ClientId, UploadDocumentsDto documents)
        {
            // 1. Validate input
            if (string.IsNullOrWhiteSpace(ClientId) || documents == null)
                return Error.Validation("بيانات غير صالحة", "يجب تقديم معرف الفني والوثائق");

            if (documents.FaceImage == null || documents.BackImage == null)
                return Error.Validation("ملفات غير مكتملة", "يجب رفع صورة الوجه وصورة الخلف");
            var sp = new ClientSpecifications(ClientId);
            // 2. Check technician
            var client = await _unitOfWork.ClientRepository.GetByIdAsync(sp);

            if (client == null)
                return Error.NotFound("الفني غير موجود", $"لا يوجد فني بالمعرف {ClientId}");

            // 3. Upload files
            var saveReFace = await _fileService.SaveFileAsync(documents.FaceImage, "ClientDocuments");
            if (!saveReFace.IsSuccess)
                return Error.Failure("فشل عمليه الحفظ", "حدث غطأ في الملفات المرسلة  يرجو الرفع مره اخري");


            var saveReBack = await _fileService.SaveFileAsync(documents.BackImage, "ClientDocuments");
            if (!saveReBack.IsSuccess)
                return Error.Failure("فشل عمليه الحفظ", "حدث غطأ في الملفات المرسلة  يرجو الرفع مره اخري");
            ;

            //Get User 
            var user = await _userManager.FindByIdAsync(ClientId);
            // 4. Create document
            var document = new UserDocument
            {
                FaceImageUrl = saveReFace.Value,
                BackImageUrl = saveReBack.Value,
                UserId = ClientId,
                User = user
            };
            client.State = StateUser.Pending;
            _unitOfWork.ClientRepository.Update(client);
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
        public async Task<Result<GetDecumentClient>> GetDocument(string id)
        {
            var sp = new ClientSpecifications(id);
            var client = await _unitOfWork.ClientRepository.GetByIdAsync(sp);
            if (client == null)
                return Error.NotFound("العميل غير موجود ", $"العميل {id} غير موجود  ");

            var doc = _mapper.Map<GetDecumentClient>(client);

            return Result<GetDecumentClient>.Ok(doc);
        }

    }
}
