using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Core.Domain
{
    public class Message    : Entity
    {
        public string Text { get; protected set; }
        public string ReceiverId { get; protected set; }
        public Guid UserId { get; protected set; }
        public bool IsRecived { get; protected set;  }

        private Message() { }

        public Message(string text, string reciverId, Guid userId, bool isRecived)
        {
            Text = text;
            ReceiverId = reciverId;
            UserId = userId;
            IsRecived = isRecived;
        }

    }
}
