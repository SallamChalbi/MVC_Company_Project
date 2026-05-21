using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.DAL.Models
{
    public class SmsMessage
    {
        public string? PhoneNumber { get; set; }
        public string? Body { get; set; }
    }
}
