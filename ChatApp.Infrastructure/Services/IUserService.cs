using ChatApp.Infrastructure.DTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Infrastructure.Services
{
    public interface IUserService
    {
        Task<AccountDto> GetAsync(Guid id);
        Task<UserDto> GetUserAsync(string connectionId);
        Task<IEnumerable<AccountDto>> BrowseAsync(Guid userId,string name = null);
        Task<IEnumerable<ConnectionDto>> GetUserConnections(Guid userId);
        Task<IEnumerable<MessageDto>> GetUserMessages(Guid userId);
        Task<IEnumerable<ActiveChatDto>> GetUserActiveChats(Guid userId);
        Task<FileStream> GetPhotoAsync(Guid id,string path);

        Task RegisterAsync(Guid userId, string email, string name, string password);
        Task<TokenDto> LoginAsync(string email, string password);
        Task AddConnection(string userId, string connectionId);
        Task AddMessage(string userId, string message, string reciverId);
        Task AddPhotoAsync(string path, Guid id, IFormFile photo);
        Task SetChatStatus(string Id,Guid userId);
        Task RemoveConnection(string connectionId);
        
    }
}
