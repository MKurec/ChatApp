using System;
using System.Collections.Generic;
using System.Text;

namespace ChatApp.Infrastructure.DTO
{
    public class JwtDto
    {
        public string Token { get; set; }
        public long Expires { get; set; }
    }
}
