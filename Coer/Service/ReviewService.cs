using AutoMapper;
using Domain.Contracts;
using Domain.Entities.OrderEntity;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Configuration;
using Service.Specifications;
using ServiceAbstraction;
using Shared;
using Shared.CommonResult;
using Shared.DTOs.ReviewDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ReviewService(IUnitOfWork _unitOfWork, 
        IHttpClientFactory _httpClientFactory,
        IConfiguration _configuration,IMapper _mapper) : IReviewService
    {
        public async Task<Result<float>> CalculateRatingDeviationAsync(string technicianId, int newRating)
        {
            var SpacRev = new ReviewSpecification(technicianId);
            var TechRevs = await _unitOfWork.ReviewRepo.GetAllAsync(SpacRev);
            if(TechRevs.Any())
            {
                return Result<float>.Ok(0);
            }
            var averageRating = (float)TechRevs.Average(r => r.Rating);
            float deviation = Math.Abs(newRating - averageRating);
            return Result<float>.Ok(deviation);
        }

        public async Task<Result<FraudCheckResponseDto>> CheckReviewFraudAsync(FraudCheckRequestDto request)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                // الرابط اللي زميلك بعته
                var response = await client.PostAsJsonAsync(_configuration.GetSection("Urls")["AI_URL"], request);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<FraudCheckResponseDto>();
                    return Result<FraudCheckResponseDto>.Ok(result);
                }

                return Error.Failure("Failed to connect to AI Service","خطا في الاتصال بخدمة الذكاء الاصطناعي");
            }
            catch (Exception ex)
            {
                // لو الـ AI وقع، بنرجع Failure عشان السيرفس الكبيرة تقرر هتعمل إيه
                return Error.Failure($"AI Service Error: {ex.Message}","حدث خطأ");
            }
        }

        public async Task<Result> CreateReviewAsync(CreateReviewDTO review)
        {
            try
            {
                var orderQuery = new OrderQuery { OrderId = review.OrderId };
                var SpacOrder = new OrderWithDetailsSpecification(orderQuery);

                var order = await _unitOfWork.OrderRepo.GetByIdAsync(SpacOrder);

                if (order == null)
                {
                    return Error.NotFound("الطلب غير موجود", $"الطلب رقم {review.OrderId} غير موجود في السجلات.");
                }

                if (order.Status != State.Completed)
                {
                    return Error.Failure("عملية غير مسموحة", "لا يمكن تقييم طلب لم يكتمل بعد.");
                }

               
                var deviationResult = await CalculateRatingDeviationAsync(order.TechnicianId, review.Rating);
                float deviation = deviationResult.IsSuccess ? deviationResult.Value : 0;
                var fraudRequest = new FraudCheckRequestDto
                {
                    rating_value = review.Rating,
                    text_length = review.Comment?.Length ?? 0,
                    rating_deviation = deviation,
                    is_night_owl = DateTime.Now.Hour >= 0 && DateTime.Now.Hour <= 5 ? 1 : 0
                };

                var aiResult = await CheckReviewFraudAsync(fraudRequest);
                var rev = new Review
                {
                    OrderId = review.OrderId,
                    Rating = review.Rating,
                    Comment = review.Comment,
                    CreatedAt = DateTime.Now, 
                    is_suspicious = aiResult.IsSuccess && (aiResult.Value?.is_suspicious ?? false),

                    FraudReasons = aiResult.IsSuccess && aiResult.Value?.FraudReasons != null
                                   ? string.Join(", ", aiResult.Value.FraudReasons)
                                   : "No suspicion detected or AI service unavailable"
                };
                await _unitOfWork.ReviewRepo.AddAsync(rev);

                var saveResult = await _unitOfWork.SaveAsync();

                if (saveResult <= 0)
                {
                    return Error.Failure("خطأ في الحفظ", "حدث خطأ أثناء حفظ التقييم في قاعدة البيانات.");
                }

                return Result.Ok();
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ (Logging) مهم هنا
                return Error.Failure("خطأ غير متوقع", $"حدث خطأ فني: {ex.Message}");
            }
        }


        public async Task<Result<IEnumerable<GetTechnicianReviews>>> GetReviewsByTechnicianIdAsync(string technicianId)
        {
            if (string.IsNullOrEmpty(technicianId))
                return Error.Failure("معرف الفني غير صحيح","الفني غير موجود");

            var spacrev = new ReviewSpecification(technicianId);

            
            var reviews = await _unitOfWork.ReviewRepo.GetAllAsync(spacrev);

            if (reviews == null)
                return Result<IEnumerable<GetTechnicianReviews>>.Ok(new List<GetTechnicianReviews>());

            var mappedReviews = _mapper.Map<IEnumerable<GetTechnicianReviews>>(reviews);

            return Result<IEnumerable<GetTechnicianReviews>>.Ok(mappedReviews);
        }
        public async Task<Result> DeleteReview(int IdReview)
        {
            try
            {
                var spec = new ReviewSpecification(IdReview);
                var review = await _unitOfWork.ReviewRepo.GetByIdAsync(spec);

                if (review == null)
                    return Error.NotFound("التقييم غير موجود", "لا يمكن حذف تقييم غير موجود.");

                _unitOfWork.ReviewRepo.Remove(review);
                var result = await _unitOfWork.SaveAsync();

                if (result <= 0)
                    return Error.Failure("فشل الحذف", "حدث خطأ أثناء محاولة حذف التقييم من قاعدة البيانات.");

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Error.Failure("خطأ غير متوقع", ex.Message);
            }
        }
        public async Task<Result<IEnumerable<GetAllReviewsDTO>>> GetAll()
        {
            try
            {
                // Specification بتعمل Include لـ Order.Client و Order.Technician
                var spec = new ReviewSpecification();
                var reviews = await _unitOfWork.ReviewRepo.GetAllAsync(spec);

                if (reviews == null)
                    return Result<IEnumerable<GetAllReviewsDTO>>.Ok(new List<GetAllReviewsDTO>());

                var mappedReviews = _mapper.Map<IEnumerable<GetAllReviewsDTO>>(reviews);
                return Result<IEnumerable<GetAllReviewsDTO>>.Ok(mappedReviews);
            }
            catch (Exception ex)
            {
                return Error.Failure("خطأ في جلب البيانات", ex.Message);
            }
        }

        public async Task<Result<GetDetailsReviewAdmin>> GetDetailsReviewAdmin(int IdReview)
        {
            try
            {
                var spec = new ReviewSpecification(IdReview);
                var review = await _unitOfWork.ReviewRepo.GetByIdAsync(spec);

                if (review == null)
                    return Error.NotFound("التقييم غير موجود", $"لا يوجد تقييم بالرقم {IdReview}");

                var mappedReview = _mapper.Map<GetDetailsReviewAdmin>(review);
                return Result<GetDetailsReviewAdmin>.Ok(mappedReview);
            }
            catch (Exception ex)
            {
                return Error.Failure("خطأ في جلب تفاصيل التقييم", ex.Message);
            }
        }


    }
}
