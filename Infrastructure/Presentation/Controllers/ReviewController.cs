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
        [HttpPost("CreateOrder")]
        [Authorize(Roles =Roles.Client)] 
        public async Task<IActionResult> Create(CreateReviewDTO reviewDto)
        {
            var result = await serviceManager.ReviewService.CreateReviewAsync(reviewDto);
            return HandleResult(result);
        }

        [HttpGet("GetTechnicianReviews/{technicianId}")]
        [Authorize(Roles = Roles.Technician)]
        public async Task<ActionResult<IEnumerable<GetTechnicianReviews>>> GetByTechnician(string technicianId)
        {
            var result = await serviceManager.ReviewService.GetReviewsByTechnicianIdAsync(technicianId);
            return HandleResult(result);
        }
        [HttpGet("GetAllReviews")]
        [Authorize(Roles = Roles.Admin)] 
        public async Task<ActionResult<IEnumerable<GetAllReviewsDTO>>> GetAllReviews()
        {
            var result = await serviceManager.ReviewService.GetAll();
            return HandleResult(result);
        }
        [HttpGet("GetReviewDetails/{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<GetDetailsReviewAdmin>> GetDetails(int id)
        {
            var result = await serviceManager.ReviewService.GetDetailsReviewAdmin(id);
            return HandleResult(result);
        }
        [HttpDelete("admin/delete/{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await serviceManager.ReviewService.DeleteReview(id);
            return HandleResult(result);
        }
    }
}
