using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.OrderEntity
{
    public enum State
    {
        pending=1, 
        accepted=2 ,
        reject = 3,
        completed =4
        
    }
}
