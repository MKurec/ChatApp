using ChatApp.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Infrastructure.Commands;

namespace ChatApp.Api.Controllers
{
    [Route("[controller]")]
    public class UsersController : DefaultController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            return Json(await _userService.GetAsync(UserId));
        }
        [HttpGet("Users")]
        [Authorize]
        public async Task<IActionResult> Get(string name)
        {
            return Json(await _userService.BrowseAsync(name));
        }
        [HttpPost("register")]
        public async Task<IActionResult> Post([FromBody] RegisterUser command)
        {
            var Id = Guid.NewGuid();
            await _userService.RegisterAsync(Id, command.Email, command.Name, command.Password);
            return Created($"/account/{Id}", null);

        }
        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody] Login command)
        {
            return Json(await _userService.LoginAsync(command.Email, command.Password));
        }
    }
}
