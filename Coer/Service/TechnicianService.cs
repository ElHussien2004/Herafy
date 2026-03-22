using AutoMapper;
using Domain.Contracts;
using Domain.Entities.UsersEntity;
using ServiceAbstraction;
using Shared.DTOs.TechnicianDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class TechnicianService(IMapper _mapper, IUnitOfWork _unitOfWork, IFileService _fileService) : ITechnicianService
    {
        public async Task<IEnumerable<TechnicianDto>> GetAllAsync()
        {
            var technicians = await _unitOfWork.TechnicalRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<TechnicianDto>>(technicians);
        }

        public async Task<TechnicianDto?> GetByIdAsync(string id)
        {
            var technician = await _unitOfWork.TechnicalRepository.GetByIdAsync(id);

            if (technician == null)
                return null;

            return _mapper.Map<TechnicianDto>(technician);
        }

        public async Task AddAsync(AddTechnicianDto technicianDto)
        {
            var technician = _mapper.Map<Technician>(technicianDto);

            await _unitOfWork.TechnicalRepository.AddAsync(technician);

            await _unitOfWork.SaveAsync();
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
            var facePath = await _fileService.UploadFile(documents.FaceImage);
            var backPath = await _fileService.UploadFile(documents.BackImage);

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

            technician.Document.FaceImageUrl = facePath;
            technician.Document.BackImageUrl = backPath;
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