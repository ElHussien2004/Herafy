using Domain.Entities.OrderEntity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared;
using Shared.DTOs.ReviewDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class ReviewController(IServiceManager  serviceManager):ApiBaseController
    {
        [HttpPost("CreateReview")]
        [Authorize(Roles =Roles.Client)] 
        public async Task<IActionResult> Create(CreateReviewDTO reviewDto)
        {
            var result = await serviceManager.ReviewService.CreateReviewAsync(reviewDto);
            return HandleResult(result);
        }

        [HttpGet("GetTechnicianReviews")]
        [Authorize(Roles = Roles.Technician+","+Roles.Client)]
        public async Task<ActionResult<IEnumerable<GetTechnicianReviews>>> GetByTechnician([FromQuery]string technicianId)
        {
            var result = await serviceManager.ReviewService.GetReviewsByTechnicianIdAsync(technicianId);
            return HandleResult(result);
        }
        [HttpGet("GetAllReviews")]
        [Authorize(Roles = Roles.Admin)] 
        public async Task<ActionResult<IEnumerable<GetDetailsReviewAdmin>>> GetAllReviews()
        {
            var result = await serviceManager.ReviewService.GetAll();
            return HandleResult(result);
        }
        [HttpGet("GetReviewDetails")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<GetDetailsReviewAdmin>> GetDetails([FromQuery]int id)
        {
            var result = await serviceManager.ReviewService.GetDetailsReviewAdmin(id);
            return HandleResult(result);
        }
        [HttpDelete("admin/deleteReview")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var result = await serviceManager.ReviewService.DeleteReview(id);
            return HandleResult(result);
        }
        [HttpPatch("ApprovedReview")]
        [Authorize(Roles=Roles.Admin)]
        public async Task<IActionResult>ActionREview([FromQuery]int ReviewId)
        {
            var result = await serviceManager.ReviewService.ApprovedReview(ReviewId);
            return HandleResult(result);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("GetCountApproved")]
        public async Task<ActionResult<int>>CountApproved()
        {
            var result = await serviceManager.ReviewService.Count_IsApproved();
            return HandleResult(result);
        }
        [Authorize(Roles = Roles.Admin)]
        [HttpGet("GetCountIs_Suspicious")]
        public async Task<ActionResult<int>> CountIs_Suspicious()
        {
            var result = await serviceManager.ReviewService.Count_Is_Suspicious();
            return HandleResult(result);
        }
    }
}
