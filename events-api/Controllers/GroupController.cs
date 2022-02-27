using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using events_api.Data;
using Microsoft.AspNetCore.Mvc;

namespace events_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly Context _context;

        public GroupController(Context context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupListDTO>>> GetGroups()
        {
            var posts = await _context.Groups.ToListAsync();
            var result = posts.ConvertAll(x => new GroupListDTO(x));
            return result;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Group>> GetGroup(Guid id)
        {
            var group = await _context.Groups.FindAsync(id);

            if (group == null)
            {
                return NotFound();
            }

            return group;
        }

    }
}
