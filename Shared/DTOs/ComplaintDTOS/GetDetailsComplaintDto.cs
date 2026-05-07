using Domain.Entities.OrderEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.ComplaintDTOS
{
    public class GetDetailsComplaintDto
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string UserRole { get; set; }

        public int? OrderId { get; set; }

        public string? Title { get; set; }

        public string Description { get; set; }

        public string? Response { get; set; }

        public ComplaintStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
