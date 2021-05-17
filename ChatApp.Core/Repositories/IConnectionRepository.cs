using ChatApp.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Core.Repositories
{
    public interface IConnectionRepository
    {
        Task<Connection> GetAsync(string id);
        Task DeleteAsync(Connection @connection);
    }
}
