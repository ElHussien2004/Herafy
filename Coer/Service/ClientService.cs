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
        public async Task<Result> AddAsync(string Userid,AddClientDto client)
        {
            if (client == null)
                return Error.Validation("بيانات غير صالحة", "يجب إدخال بيانات العميل");

            var user = await _userManager.FindByIdAsync(Userid);

            if (user == null)
                return Error.NotFound("المستخدم غير موجود", $"لم يتم العثور على مستخدم بالمعرف {Userid}");
            var sp = new ClientSpecifications(null);

            var existing = await _unitOfWork.ClientRepository.GetByIdAsync(sp);

            if (existing != null)
                return Error.Validation("موجود بالفعل", "هذا المستخدم مسجل بالفعل كعميل");

            string? imageUrl = null;

            if (client.ProfileImageURL != null && client.ProfileImageURL.Length > 0)
            {
                var uploadResult = await _fileService.SaveFileAsync(client.ProfileImageURL, "ProfileImage");

                if (!uploadResult.IsSuccess)
                    return uploadResult;

                imageUrl = uploadResult.Value;
            }

            user.FullName = client.FullName;

            if (imageUrl != null)
                user.ProfileImageURL = imageUrl;

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
                return Error.Failure("فشل تحديث المستخدم", "حدث خطأ أثناء تحديث بيانات المستخدم");

            var clientEntity = _mapper.Map<Client>(client);

            await _unitOfWork.ClientRepository.AddAsync(clientEntity);

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

        public async Task<Result<bool>> ChangeIsActive(string id ,bool state)
        {
            //get client 
            if (id == null)
                return Result<bool>.Fail( Error.Validation("بيانات غير صالحة", "يجب إدخال بيانات العميل"));

            var sp = new ClientSpecifications(id);
            var client =await _unitOfWork.ClientRepository.GetByIdAsync(sp);

            if (client == null)
                    return Error.NotFound("العميل غير موجود", $"لا يوجد العميل بالمعرف {id}");
            // is active false 

            client.IsActive = state;
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

        public async Task<Result<ClientDto?>> GetByIdAsync(string id)
        {
            var sp = new ClientSpecifications();
            var client = await _unitOfWork.ClientRepository.GetByIdAsync(sp);
            if (client == null)
                return Error.NotFound("العميل غير موجود ", $"العميل {id} غير موجود  ");

            return Result<ClientDto?>.Ok(_mapper.Map<ClientDto>(client));
        }
    }
}
