using Domain.Entities.OrderEntity;
using Shared.CommonResult;
using Shared.DTOs.ReviewDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface IReviewService
    {
        Task<Result> CreateReviewAsync(CreateReviewDTO review);
        Task<Result<float>> CalculateRatingDeviationAsync(string technicianId, int newRating);
        Task<Result<FraudCheckResponseDto>> CheckReviewFraudAsync(FraudCheckRequestDto request);
        Task<Result<IEnumerable<GetTechnicianReviews>>> GetReviewsByTechnicianIdAsync(string technicianId);

        Task<Result<IEnumerable<GetAllReviewsDTO>>> GetAll();
        Task<Result<GetDetailsReviewAdmin>>GetDetailsReviewAdmin(int IdReview);

        Task<Result>DeleteReview(int IdReview);

    }
}
