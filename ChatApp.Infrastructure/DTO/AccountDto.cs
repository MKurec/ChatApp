using System;
using System.Collections.Generic;
using System.Text;

namespace ChatApp.Infrastructure.DTO
{
    public class AccountDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool? IsChatActive { get; set; }
    }
}
