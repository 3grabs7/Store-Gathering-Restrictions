using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopGatheringRestrictionsApi.Controllers
{

    // enter ++ section timestamp
    // exit -- section timestamp
    // number of people in section = enter - exit



    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public RegisterController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("[Action]/{section}")]
        public async Task<ActionResult> Enter(string section)
        {


            return Ok();
        }

        [HttpGet("[Action]/{section}")]
        public async Task<ActionResult> Exit(string section)
        {


            return Ok();
        }

    }
}
