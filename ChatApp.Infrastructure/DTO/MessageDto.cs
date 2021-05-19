using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Infrastructure.DTO
{
    public class MessageDto
    {
        public string Text { get;  set; }
        public string ReceiverId { get;  set; }
        public bool IsRecived { get;  set; }
    }
}
