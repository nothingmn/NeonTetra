using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NeonTetra.Contracts.Membership;

namespace NeonTetraWebApi.Controllers
{
    [Route("api/[controller]/{id?}")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager _userManager;

        public UserController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IUser> Get(string id)
        {
            return await _userManager.Get(id);
        }
    }
}