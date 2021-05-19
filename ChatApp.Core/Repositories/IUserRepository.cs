using ChatApp.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Core.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetAsync(Guid id);
        Task<User> GetAsync(string name);
        Task<User> GetByEmailAsync(string name);
        Task<IEnumerable<User>> BrowseAsync(string name = "");

        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task UpdateMessagesAsync(User @user);
        Task DeleteAsync(User user);

    }
}
