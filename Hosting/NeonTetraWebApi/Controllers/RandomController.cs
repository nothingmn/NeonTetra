using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NeonTetra.Contracts.Membership;
using NeonTetra.Contracts.Services;

namespace NeonTetraWebApi.Controllers
{
    [Route("api/[controller]/{id?}")]
    [ApiController]
    public class RandomController : ControllerBase
    {
        private readonly IRandomProvider _rnd;

        public RandomController(IRandomProvider rnd)
        {
            _rnd = rnd;
        }

        [HttpGet]
        public object Get()
        {
            return new
            {
                insecure = _rnd.Generate(),
                secure = _rnd.Generate(RandomType.Secure),
                secureString = _rnd.Generate(RandomType.Secure),
                insecureSring = _rnd.GenerateString(RandomType.InSecure),
                secureInt = _rnd.GenerateInt(RandomType.Secure),
                secureDouble = _rnd.GenerateDouble(RandomType.Secure)
            };
        }
    }
}