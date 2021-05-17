using AutoMapper;
using ChatApp.Core.Domain;
using ChatApp.Core.Repositories;
using ChatApp.Infrastructure.DTO;
using ChatApp.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtHandler _jwtHandler;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, IJwtHandler jwtHandler, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _jwtHandler = jwtHandler;
        }
        public async Task<IEnumerable<AccountDto>> BrowseAsync(string name = null)
        {
            var @users = await _userRepository.BrowseAsync(name);
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
            // var @user = @users.SingleOrDefault(x => x.Connections.Any(x => x.ConnectionId == connectionId) != false);
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
            

        public async Task<TokenDto> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetAsync(email);
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
        public async Task RemoveConnection(string connectionId)
        {
            var @users = await _userRepository.BrowseAsync("");
            var @user = @users.SingleOrDefault(x => x.Connections.Any(x => x.ConnectionId == connectionId) != false);
            if (users == null) throw new Exception($"No users with '{connectionId}' in database");
            @user.Connections.Remove(@user.Connections.Single(x => x.ConnectionId == connectionId));
            await _userRepository.UpdateAsync(@user);
        }
    }
}
