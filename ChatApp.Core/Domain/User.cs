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
        //public bool? IsActiv { get; protected set; }
        public ICollection<Connection> Connections { get; protected set; }
        public ICollection<Message> Messages { get; protected set; }
        public string? ImageLocation { get; protected set; }
        public ICollection<ActiveChat> ActiveChats { get; protected set; }
        public ICollection<User> UserFriends { get; protected set; }
        public ICollection<User> UnconfirmedFriends { get; protected set; }

        protected User() { }

        public User(Guid id, string name, string password, string email)
        {
            Id = id;
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
        public void SetChatStatus(bool isActiv,string userId)
        { 
            if(ActiveChats.SingleOrDefault(x => x.UserId== userId)==null)
            {
                ActiveChats.Add(new ActiveChat(userId,isActiv));
            }
            else
            {
                var activChat = ActiveChats.SingleOrDefault(x => x.UserId == userId);
                activChat.SetFlag(isActiv);
            }
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
        public void SetImageLocation(String imageLocation)
        {
            if (string.IsNullOrEmpty(imageLocation))
            {
                throw new Exception($"User with id : '{this.Id}' can not have empty image location");
            }
            ImageLocation = imageLocation;
        }

        public void AddFriend(User user)
        {
            if(user==null)
            {
                throw new Exception("Can not add non existing user to Friend's");
            }
            if(UserFriends.Contains(user))
            {
                throw new Exception($"User {user.Name} with id {user.Id} is alredy on Fiends list");
            }
            UserFriends.Add(user);
        }

        public void RemoveFriendFromFriends(User user)
        {
            if (!UserFriends.Contains(user))
            {
                throw new Exception($"Cannot remove user {user.Name} from friends list, it does't contains this user ");
            }
            UserFriends.Remove(user);
        }
        
        public void AddFriendForConfirmation(User user)
        {
            if (user == null)
            {
                throw new Exception("Can not add non existing user to Confirmation list");
            }
            if (UnconfirmedFriends.Contains(user))
            {
                throw new Exception($"User {user.Name} with id {user.Id} is alredy on Confirmation list");
            }
            UnconfirmedFriends.Add(user);
        }

        public void RemoveFromUnconfirmedFriends(User user)
        {
            if (!UserFriends.Contains(user))
            {
                throw new Exception($"Cannot remove user {user.Name} from friends list, it does't contains this user ");
            }
            UserFriends.Remove(user);
        }

    }
}
