using AutoMapper;
using ChatApp.Core.Domain;
using ChatApp.Core.Repositories;
using ChatApp.Infrastructure.DTO;
using ChatApp.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.IO;

namespace ChatApp.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtHandler _jwtHandler;
        private readonly IMapper _mapper;
        private readonly IFileRepository _fileRepository;
        public UserService(IUserRepository userRepository, IJwtHandler jwtHandler, IMapper mapper, IFileRepository fileRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _jwtHandler = jwtHandler;
            _fileRepository = fileRepository;
        }
        public async Task<IEnumerable<AccountDto>> BrowseAsync(Guid userId, string name = null)
        {
            var @users = await _userRepository.BrowseAsync(name);
            var @thisuser = await _userRepository.GetAsync(userId);
            if (thisuser == null) throw new Exception($"User with id : {userId} dosen't exist");
            var @activeChats = thisuser.ActiveChats.AsEnumerable();
            IEnumerable<AccountDto> jointusers = (from user in users
                             join activeChat in activeChats on user.Id.ToString() equals activeChat.UserId into gr
                             from output in gr.DefaultIfEmpty()
                             select new AccountDto() {Id= user.Id,Name= user.Name,Email= user.Email, IsChatActive = output?.IsActiv });
            return jointusers;
        }

        public async Task<IEnumerable<AccountDto>> BrowseFriendsAsync(Guid userId)
        {
            var @thisuser = await _userRepository.GetAsync(userId);
            if (thisuser == null) throw new Exception($"User with id : {userId} dosen't exist");
            var users = thisuser.UserFriends;
            var @activeChats = thisuser.ActiveChats.AsEnumerable();
            IEnumerable<AccountDto> jointusers = (from user in users
                                                  join activeChat in activeChats on user.Id.ToString() equals activeChat.UserId into gr
                                                  from output in gr.DefaultIfEmpty()
                                                  select new AccountDto() { Id = user.Id, Name = user.Name, Email = user.Email, IsChatActive = output?.IsActiv });
            return jointusers;
        }

        public async Task<IEnumerable<AccountDto>> BrowseInvitationsAsync(Guid userId)
        {
            var @thisuser = await _userRepository.GetAsync(userId);
            if (thisuser == null) throw new Exception($"User with id : {userId} dosen't exist");
            var users = thisuser.UnconfirmedFriends;
            return _mapper.Map<IEnumerable<AccountDto>>(users);
        }

        public async Task<AccountDto> GetAsync(Guid id)
        {
            var account = await _userRepository.GetAsync(id);
            return _mapper.Map<AccountDto>(account);
        }

        public async Task<UserDto> GetUserAsync(string connectionId)
        {
            var @users = await _userRepository.BrowseAsync("");
            var @user = @users.FirstOrDefault().Connections.Any(x => x.ConnectionId == connectionId);
            if (users != null) throw new Exception($"No users with '{connectionId}' in database");
            return _mapper.Map<UserDto>(@user);
        }

        public async Task<IEnumerable<ConnectionDto>> GetUserConnections(Guid userId)
        {
            var @user = await _userRepository.GetAsync(userId);
            if (user == null) throw new Exception($"User with id : {userId} dosen't exist");
            var @Connections = user.Connections.AsEnumerable();
            return _mapper.Map<IEnumerable<ConnectionDto>>(Connections);
        }

        public async Task<IEnumerable<MessageDto>> GetUserMessages(Guid userId)
        {
            var @user = await _userRepository.GetAsync(userId);
            if (user == null) throw new Exception($"User with id : {userId} dosen't exist");
            var @mesages = user.Messages.AsEnumerable();
            return _mapper.Map<IEnumerable<MessageDto>>(mesages);
        }

        public async Task<IEnumerable<ActiveChatDto>> GetUserActiveChats(Guid userId)
        {
            var @user = await _userRepository.GetAsync(userId);
            if (user == null) throw new Exception($"User with id : {userId} dosen't exist");
            var @activeChats = user.ActiveChats.AsEnumerable();
            return _mapper.Map<IEnumerable<ActiveChatDto>>(activeChats);
        }

        public async Task<FileStream> GetPhotoAsync(Guid id,string defaultpath)
        {
            var @user = await _userRepository.GetAsync(id);
            var @image = await _fileRepository.GetAsync(@user.ImageLocation, defaultpath);
            return image;

        }


        public async Task<TokenDto> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                throw new Exception("Invalid credentials.");
            }
            if (user.Password != password)
            {
                throw new Exception("Invalid credentials.");
            }
            var jwt = _jwtHandler.CreateToken(user.Id);

            return new TokenDto
            {
                Token = jwt.Token,
                Expires = jwt.Expires
            };
        }

        public async Task RegisterAsync(Guid userId, string email, string name, string password)
        {
            var user = await _userRepository.GetAsync(name);
            if (user != null) throw new Exception($"User with name: '{name}' alredy exist");
            user = await _userRepository.GetByEmailAsync(email);
            if (user != null) throw new Exception($"User with email: '{email}' alredy exist");
            user = new User(userId, name, password, email);
            await _userRepository.AddAsync(user);
        }

        public async Task AddConnection(string userId, string connectionId)
        {
            var Id = new Guid(userId);
            var @user = await _userRepository.GetAsync(Id);
            if (user == null) throw new Exception($"User with id: '{userId}' don't exist");
            @user.AddConnection(connectionId);
            await _userRepository.UpdateAsync(@user);
        }
        public async Task SetChatStatus(string Id,Guid userId)
        {
            var @user = await _userRepository.GetAsync(userId);
            if (user == null) throw new Exception($"User with id: '{userId}' don't exist");
            var activchat = @user.ActiveChats.SingleOrDefault(x => x.UserId == Id);
            if (activchat != null)
            {
                activchat.SetFlag(false);
                await _userRepository.UpdateMessagesAsync(@user);
            }
        }

        public async Task AddMessage(string userId, string message, string reciverId)
        {
            var Id = new Guid(userId);
            var @user = await _userRepository.GetAsync(Id);
            if (user == null) throw new Exception($"User with id: '{userId}' don't exist");
            user.AddMessage(message,reciverId,false);
            await _userRepository.UpdateMessagesAsync(@user);
            var ReciverId = new Guid(reciverId);
            var @reciver = await _userRepository.GetAsync(ReciverId);
            if (reciver == null) throw new Exception($"User with id: '{reciverId}' don't exist");
            reciver.AddMessage(message, userId, true);
            reciver.SetChatStatus(true, userId);
            await _userRepository.UpdateMessagesAsync(@reciver);
            
        }
        public async Task RemoveConnection(string connectionId)
        {
            var @users = await _userRepository.BrowseAsync("");
            var @user = @users.SingleOrDefault(x => x.Connections.Any(x => x.ConnectionId == connectionId) != false);
            if (users == null) throw new Exception($"No users with '{connectionId}' in database");
            @user.Connections.Remove(@user.Connections.Single(x => x.ConnectionId == connectionId));
            await _userRepository.UpdateAsync(@user);
        }
        public async Task AddPhotoAsync(string path, Guid id, IFormFile photo)
        {
            var @user = await _userRepository.GetAsync(id);
            int width = 0;
            var productImage = Image.Load(photo.OpenReadStream());
            int div = productImage.Height / 256;
            int hight = productImage.Height / div;
            if (productImage.Height < productImage.Width) width = 256;
            else width = productImage.Width / div;
            productImage.Mutate(x => x.Resize(width, hight));
            await _fileRepository.AddAsync(productImage, path, user.Id);
            user.SetImageLocation(Path.Combine(path + "\\uploads\\" + user.Id + ".png"));
            await _userRepository.UpdateAsync(@user);
        }

        public async Task AddFriendToList(Guid userId,Guid friendId)
        {
            var @user = await _userRepository.GetAsync(userId);
            var @friend = await _userRepository.GetAsync(friendId);
            if (friend == null) throw new Exception($"Friend with id {friendId} doesn't exist ");
            if (user.UserFriends.Contains(friend)) throw new Exception($"{friend.Name} is already on your Friends list");
            if(user.UnconfirmedFriends.Contains(@friend))
            {
                user.RemoveFromUnconfirmedFriends(friend);
                user.AddFriend(friend);
                if (friend.UserFriends.Contains(user)) throw new Exception($"You are already on {user.Name} Friends list");
                friend.AddFriend(user);
            }
            else
            {
                if (friend.UnconfirmedFriends.Contains(user)) throw new Exception($"{friend.Name} is alredy waiting to confirm your invitation ");
                friend.AddFriendForConfirmation(user);
            }
            await _userRepository.UpdateAsync(@user);
            await _userRepository.UpdateAsync(@friend);
        }

        public async Task RemoveFriendFromList(Guid userId, Guid friendId)
        {
            var @user = await _userRepository.GetAsync(userId);
            var @friend = await _userRepository.GetAsync(friendId);
            if (friend == null) throw new Exception($"Friend with id {friendId} doesn't exist ");
            if (!user.UserFriends.Contains(friend)) throw new Exception($"Your Friends list doesn't contain {friend.Name} with id {friend.Id}");
            @user.UserFriends.Remove(@friend);
            @friend.RemoveFriendFromFriends(@user);
            await _userRepository.UpdateAsync(@user);
            await _userRepository.UpdateAsync(@friend);

        }
    }
}
