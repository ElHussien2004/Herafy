using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.OrderDtos
{
    public class GetAllOrderDTO
    {
        public int Id { get; set; }
        public string NameTec {  get; set; }
        public string NameCli { get; set; }
        public string ServiceName { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal FinalPrice { get; set; }

        public string State { get; set; }
    }
}
