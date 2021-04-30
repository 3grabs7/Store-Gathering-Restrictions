using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopGatheringRestrictionsApi.Models;
using System;
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

        [HttpGet("[action]/{store}")]
        public async Task<ActionResult<Store>> Get(string store)
        {
            var result = await _context.Stores
                .Include(s => s.Sections)
                    .ThenInclude(s => s.Enters)
                .Include(s => s.Sections)
                    .ThenInclude(s => s.Exits)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Name.Equals(store));

            return Ok(result);
        }

        [HttpPost("[action]/{logging}")]
        public async Task<ActionResult> Log(Logging logging)
        {
            var store = await _context.Stores
                .FirstOrDefaultAsync(s => s.Name == logging.Store);
            if (store == default)
            {
                return NotFound(new { Message = $"Store '{logging.Store}' not found." });
            }

            var section = await _context.Sections
                .FirstOrDefaultAsync(s => s.Name == logging.Section && s.Store == store);
            if (section == default)
            {
                return NotFound(new { Message = $"Section '{logging.Section}' not found." });
            }

            switch (logging.Direction)
            {
                case "enter":
                    {
                        var log = new Enter()
                        {
                            Section = section,
                            TimeStamp = DateTime.Now,
                        };
                        await _context.AddAsync(log);
                        break;
                    }
                case "exit":
                    {
                        var log = new Exit()
                        {
                            Section = section,
                            TimeStamp = DateTime.Now,
                        };
                        await _context.AddAsync(log);
                        break;
                    }
                default:
                    return BadRequest(new { Message = $"Value '{logging.Direction}' for param 'direction' was invalid." });
            }
            await _context.SaveChangesAsync();
            return Ok(new { Message = $"Log saved." });
        }


    }
}
