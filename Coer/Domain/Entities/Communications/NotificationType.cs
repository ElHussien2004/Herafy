using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Communications
{
    public enum NotificationType
    {
        NewOrder=1,
        OrderAccepted=2,
        OrderCompleted=3,
        NewMessage=4
    }
}
