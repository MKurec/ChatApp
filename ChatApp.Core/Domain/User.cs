using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Core.Domain
{
    public class User : Entity
    {
        public string Name { get; protected set; }
        public string Email { get; protected set; }
        public string Password { get; protected set; }
        public ICollection<Connection> Connections { get; protected set; }

        protected User() { }

        public User(Guid Id, string name, string password, string email)
        {
            SetName(name);
            SetPassword(password);
            SetEmail(email);
        }
        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new Exception($"User can not have an empty name.");
            }
            Name = name;
        }
        public void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new Exception($"User can not have an empty email.");
            }
            Email = email;
        }
        public void SetPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new Exception($"User can not have an empty name.");
            }
            Password = password;
        }

    }
}
