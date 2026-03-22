using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Wapper
{
    public class Response<T>
    {
        public string Message { get; set; }
        public T Data { get; set; }
        public bool Success { get; set; }
        public DateTime DateTime { get; set; }
        public Response()
        {
            DateTime = DateTime.Now;
        }
        public Response(T data, string message) : this() 
        {
            Data = data;
            Message = message;
            Success = true;
        }
        public Response(string message) : this()
        {
             Message = message;
             Success = false;
        }
    }
}
