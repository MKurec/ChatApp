using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Core.Domain
{
    public class Connection  
    {
        [Key]
        public string ConnectionId { get; protected set; }
        public bool Connected { get; protected set; }
        public Guid UserId { get; protected set; }

        protected Connection() { }

        public Connection(string connectionId, Guid userId)
        {
            SetConnectionId(connectionId);
            Connected = true;
            UserId = userId;

        }
        public void SetConnectionId(string connectionId)
        {
            if (string.IsNullOrWhiteSpace(connectionId))
            {
                throw new Exception($"Trying to set empty connectionId.");
            }
            ConnectionId = connectionId;
        }
    }
    
}
