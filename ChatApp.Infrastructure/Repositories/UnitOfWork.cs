using ChatApp.Core;
using ChatApp.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AplicationDbContext _context;

        public UnitOfWork(AplicationDbContext context)
        {
            _context = context;
            Users = new UserRepository(_context);
            Connections = new ConnectionRepository(_context);
        }

        public IUserRepository Users { get; protected set; }
        public IConnectionRepository Connections { get; protected set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
