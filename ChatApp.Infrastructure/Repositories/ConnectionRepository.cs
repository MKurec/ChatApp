using ChatApp.Core.Domain;
using ChatApp.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Infrastructure.Repositories
{
    public class ConnectionRepository : IConnectionRepository
    {
        protected readonly DbContext Context;
        private DbSet<Connection> _connections;
        public ConnectionRepository(DbContext context)
        {
            this.Context = context;
            this._connections = Context.Set<Connection>();
        }
        public async Task DeleteAsync(Connection connection)
        {
            _connections.Remove(connection);
            Context.SaveChanges();
            await Task.CompletedTask;
        }

        public async Task<Connection> GetAsync(string id)
        {
            var @connection = await Task.FromResult(_connections.SingleOrDefault(x => x.ConnectionId == id));
            return connection;
        }
    }
}
