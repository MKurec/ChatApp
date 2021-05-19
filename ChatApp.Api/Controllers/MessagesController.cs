using ChatApp.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Api.Controllers
{
    [Route("[controller]")]
    public class MessagesController : DefaultController
    {
        private readonly IUserService _userService; 

        public MessagesController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            return Json(await _userService.GetUserMessages(UserId));
        }
    }
    
}
