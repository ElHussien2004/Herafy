using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.OrderEntity
{
    public enum ComplaintStatus
    {
        Submitted = 0,
        UnderReview = 1,
        Resolved = 2,
        Rejected = 3
    }
}
