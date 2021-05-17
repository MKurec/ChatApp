using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Infrastructure.DTO
{
    public class ConnectionDto
    {
        public string ConnectionId { get;  set; }
        public bool Connected { get;  set; }
        public Guid UserId { get;  set; }
    }
}
