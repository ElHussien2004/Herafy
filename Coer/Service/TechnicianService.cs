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
    public class TechnicianService(IUnitOfWork _unitOfWork, IFileService _fileService) : ITechnicianService
    {
        public async Task<bool> UploadDocumentsAsync(string technicianId, UploadDocumentsDto documents)
        {
            if (documents.FaceImage == null || documents.BackImage == null)
                throw new ArgumentException("Both images are required");
            var technician = await _unitOfWork.TechnicalRepository.GetByIdAsync(technicianId);
            if (technician == null)
                return false;
            // لو في صور قديمة احذفها
            if (technician.Document != null)
            {
                if (!string.IsNullOrEmpty(technician.Document.FaceImageUrl))
                    await _fileService.DeleteFile(technician.Document.FaceImageUrl);

                if (!string.IsNullOrEmpty(technician.Document.BackImageUrl))
                    await _fileService.DeleteFile(technician.Document.BackImageUrl);
            }
            // رفع الصور الجديدة
            var facePath = await _fileService.UploadFile(documents.FaceImage);
            var backPath = await _fileService.UploadFile(documents.BackImage);
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

            _unitOfWork.TechnicalRepository.Update(technician);

            await _unitOfWork.SaveAsync();

            return true;
        }
        public async Task<bool> ToggleAvailabilityStatusAsync(string technicianId, bool isAvailable)
        {
            var technician = await _unitOfWork.TechnicalRepository
               .GetByIdAsync(technicianId);

            if (technician == null)
                return false;
            //Updating State
            technician.AvailabilityStatus = isAvailable;
            _unitOfWork.TechnicalRepository.Update(technician);
            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}