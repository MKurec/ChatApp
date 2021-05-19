using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Core.Domain
{
    public class ActiveChat
    {
        public string UserId { get; protected set; }
        public bool? IsActiv { get; protected set; }

        protected ActiveChat() { }

        public ActiveChat(string userId,bool isActiv)
        {
            UserId = userId;
            IsActiv = isActiv; 
        }
        public void SetFlag(bool isActiv)
        {
            IsActiv = isActiv;
        }
            
    }
}
