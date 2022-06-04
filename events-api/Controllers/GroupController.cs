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
            _context.Qualifications.Load();
        }

        [HttpPost("search")]
        public async Task<ActionResult<IEnumerable<GroupDTO>>> SearchGroups(GroupFilterDTO groupFilterDTO)
        {
           
            IQueryable<Group> q = _context.Groups.Include(x => x.Qualification);

            if (groupFilterDTO.QualificationIds !=null && groupFilterDTO.QualificationIds.Any())
            { 
                var _guids = groupFilterDTO.QualificationIds.Select(z => Guid.ParseExact(z, "D")).ToList();
                q = q.Where(x => _guids.Any(z => z == x.QualificationId));
            }

            if (!String.IsNullOrEmpty(groupFilterDTO.SearchTerm))
            {
                q = q.Where(x => x.Name.Contains(groupFilterDTO.SearchTerm));
            }
            return await q.Select(group => new GroupDTO(group)).ToListAsync();

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupListDTO>>> GetGroups()
        {
            var posts = await _context.Groups.ToListAsync();
            var result = posts.ConvertAll(x => new GroupListDTO(x));
            return result;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<GroupGetDTO>> GetGroup(string id)
        {
            var group = await _context.Groups.FindAsync(Guid.ParseExact(id, "D"));

            if (group == null)
            {
                return NotFound();
            }
            var groupGet = new GroupGetDTO
            {
                Id = group.Id.ToString(),
                Name = group.Name,
                Date = group.Date,
                QualificationId = group.QualificationId.ToString(),
           
            };
            return groupGet;
        }


        [HttpPost]
        public async Task<ActionResult<string>> PostGroup(GroupPostDTO groupDto)
        {
            var qual_id = Guid.ParseExact(groupDto.QualificationId, "D");
            Qualification qual = _context.Qualifications.Find(qual_id);

            if (qual == null)
            {
                return NotFound();
            }
            var group = new Group
            {
                Name = groupDto.Name,
                QualificationId = qual.Id,
                Qualification = qual,
                Date = groupDto.Date.ToUniversalTime()

            };
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();

            return group.Id.ToString();
        }

        // DELETE: api/group/5
        [HttpPost("delete")]
        public async Task<IActionResult> PostGroupsDelete(List<string> ids)
        {
            var _guids = ids.Select(z => Guid.ParseExact(z, "D"));
            var _groups = _context.Groups.Where(x => _guids.Any(z => z == x.Id)).ToList();

            _context.Groups.RemoveRange(_groups);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        // PUT: api/group/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroup(string id, GroupPutDTO group)
        {
            Group _group = _context.Groups.Find(Guid.ParseExact(id, "D"));
         

            if (_group == null)
            {
                return NotFound();
            }
      
            _group.Name = group.Name;
            _group.QualificationId = group.QualificationId;
            _group.Date = group.Date.ToUniversalTime();
            _group.Qualification = _context.Qualifications.FirstOrDefault(x => x.Id == group.QualificationId);

            _context.Entry(_group).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                throw;
            }

            return NoContent();
        }

    }
}
