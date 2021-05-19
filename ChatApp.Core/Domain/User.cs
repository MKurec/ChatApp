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
        public ICollection<Message> Messages { get; protected set; }

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
            if (!email.Contains("@"))
            {
                throw new Exception($"Invalid email");
            }
            Email = email;
        }
        public void SetPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new Exception($"User can not have an empty password.");
            }
            Password = password;
        }
        public void AddConnection(string connectionId)
        {
            if (string.IsNullOrEmpty(connectionId))
            {
                throw new Exception("Couldn't add connection with no connection Id");
            }
            Connections.Add(new Connection(connectionId,this.Id));
        }
        public void AddMessage(string text,string reciverId, bool isRecived)
        {
            if(string.IsNullOrEmpty(text))
            {
                throw new Exception("You couldn't send empty message");
            }
            Messages.Add(new Message(new Guid() ,text, reciverId, this.Id, isRecived));
        }

    }
}
