using Shared.CommonResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface ISMSService
    {
        public Task<Result> SendAsync(string to, string message);
    }
}
