using ChatApp.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Infrastructure.Commands;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace ChatApp.Api.Controllers
{
    [Route("[controller]")]
    public class UsersController : DefaultController
    {
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _environment;

        public UsersController(IUserService userService, IWebHostEnvironment webHostEnvironment)
        {
            _userService = userService;
            _environment = webHostEnvironment;
        }
        public class FIleUploadAPI
        {
            public IFormFile photo { get; set; }
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
            return Json(Id);

        }
        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody] Login command)
        {
            return Json(await _userService.LoginAsync(command.Email, command.Password));
        }
        [HttpPost("AddPhoto")]
        [Authorize]
        public async Task<IActionResult> UploadFile([FromHeader] FIleUploadAPI file)
        {
            if (file.photo.ToString().Length > 0)
            {
                string path = _environment.WebRootPath.ToString();
                await _userService.AddPhotoAsync(path, UserId, file.photo);
            }
            return Created($"Image/{UserId}", null);

        }
        [Route("Image/{userId}")]
        [HttpGet]
        public async Task<IActionResult> Get(Guid userId, bool a)
        {
            string path = _environment.WebRootPath.ToString();
            var @image = await _userService.GetPhotoAsync(userId,path);
            
            if (image == null)
            {
                return NotFound();
            }
            return File(@image, "image/jpeg");

        }
    }
}
