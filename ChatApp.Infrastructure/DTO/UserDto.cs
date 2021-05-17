using ChatApp.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatApp.Infrastructure.DTO
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public ICollection<Connection> Connections { get;  set; }
    }
}
