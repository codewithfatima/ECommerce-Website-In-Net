using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Models
{
    public class Whislist
    {
       public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int ProductId { get; set; }  
    }
}
