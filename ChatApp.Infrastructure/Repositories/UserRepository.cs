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
    public class UserRepository : IUserRepository
    {
        protected readonly DbContext Context;
        private DbSet<User> _users;
        public UserRepository(DbContext context)
        {
            this.Context = context;
            this._users = Context.Set<User>();
        }


        public async Task AddAsync(User user)
        {
            _users.Add(user);
            Context.SaveChanges();
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<User>> BrowseAsync(string name = "")
        {
            var users = _users.Include(x => x.Connections).AsEnumerable();
            if (!string.IsNullOrEmpty(name))
            {
                users = users.Where(x => x.Name.ToUpperInvariant().Contains(name.ToUpperInvariant()));
            }
            return await Task.FromResult(users);
        }

        public async Task DeleteAsync(User user)
        {
            _users.Remove(user);
            Context.SaveChanges();
            await Task.CompletedTask;
        }

        public async Task<User> GetAsync(Guid id)
        {
            var @user = await Task.FromResult(_users.Include(x => x.Connections).Include(x => x.Messages).SingleOrDefault(x => x.Id == id));
            return user;
        }

        public async Task<User> GetAsync(string name)
        {
            var @user = await Task.FromResult(_users.SingleOrDefault(x => x.Name.ToLower() == name.ToLower()));
            return user;
        }
        public async Task<User> GetByEmailAsync(string email)
        {
            var @user = await Task.FromResult(_users.SingleOrDefault(x => x.Email.ToLower() == email.ToLower()));
            return user;
        }

        public async Task UpdateAsync(User @user)
        {
            Context.Entry(@user).State = EntityState.Modified;
            Context.SaveChanges();
            await Task.CompletedTask;
        }

        public async Task UpdateMessagesAsync(User @user)
        {
            Context.Attach(@user);
            Context.Entry(@user).Collection(x => x.Messages).IsModified=true;
            Context.SaveChanges();
            await Task.CompletedTask;
        }
    }
}
