﻿using ChatApp.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Infrastructure.Services
{
    public interface IUserService
    {
        Task<AccountDto> GetAsync(Guid id);
        Task<UserDto> GetUserAsync(string connectionId);
        Task<IEnumerable<AccountDto>> BrowseAsync(string name = null);
        Task<IEnumerable<ConnectionDto>> GetUserConnections(Guid userId);

        Task RegisterAsync(Guid userId, string email, string name, string password);
        Task<TokenDto> LoginAsync(string email, string password);
        Task AddConnection(string userId, string connectionId);
        Task RemoveConnection(string connectionId);
        
    }
}
