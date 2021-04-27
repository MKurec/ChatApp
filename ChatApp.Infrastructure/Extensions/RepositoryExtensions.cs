using ChatApp.Core.Domain;
using ChatApp.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Infrastructure.Extensions
{
    public static class RepositoryExtensions
    {
        public static async Task<User> GetOrFailAsync(this IUserRepository repository, Guid id)
        {
            var user = await repository.GetAsync(id);
            if (user == null)
            {
                throw new Exception($"User with id: '{id}' does not exist.");
            }

            return user;
        }

    }
}
