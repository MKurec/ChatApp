using ChatApp.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IConnectionRepository Connections { get; }
        int Complete();
    }
}
