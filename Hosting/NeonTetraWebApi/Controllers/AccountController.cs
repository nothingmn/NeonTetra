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
    public class AccountController : ControllerBase
    {
        private readonly IAccountManager _authManager;

        public AccountController(IAccountManager authManager)
        {
            _authManager = authManager;
        }

        /// <summary>
        /// Get profile
        /// </summary>
        /// <param name = "id" ></ param >
        /// < returns ></ returns >
        [HttpGet]
        public Task<IUser> Get()
        {
            return Task.FromResult(default(IUser));
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IUser> Post(string username, string password)
        {
            return await _authManager.Login(username, password);
        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[HttpDelete]
        //public async Task<IUser> Delete(string id)
        //{
        //    return await _userManager.Logout(id);
        //}
    }
}