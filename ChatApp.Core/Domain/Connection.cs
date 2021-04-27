using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Core.Domain
{
    public class Connection   :Entity
    {
        public string UserAgent { get; protected set; }
        public bool Connected { get; protected set; }
    }
}
