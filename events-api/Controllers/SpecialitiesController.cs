#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using events_api.Data;

namespace events_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecialitiesController : ControllerBase
    {
        private readonly Context _context;

        public SpecialitiesController(Context context)
        {
            _context = context;
            _context.Qualifications.Load();
            _context.Groups.Load();
        }

        [HttpPost("search")]
        public async Task<ActionResult<IEnumerable<SpecialityDTO>>> SearchSpecialitys(SpecialityFilterDTO specialityFilterDTO)
        {
            IQueryable<Speciality> q = _context.Specialities;

            if (!String.IsNullOrEmpty(specialityFilterDTO.SearchTerm))
            {
                q = q.Where(x => x.Name.Contains(specialityFilterDTO.SearchTerm));
            }
            return await q.Select(speciality => new SpecialityDTO(speciality)).ToListAsync();

        }


        // GET: api/Specialities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Speciality>>> GetSpecialities()
        {
            return await _context.Specialities.ToListAsync();
        }

        // GET: api/Specialities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Speciality>> GetSpeciality(Guid id)
        {
            var speciality = await _context.Specialities.FindAsync(id);

            if (speciality == null)
            {
                return NotFound();
            }

            return speciality;
        }

        // PUT: api/Specialities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSpeciality(string id, SpecialityPostDTO speciality)
        {
            Speciality _speciality = _context.Specialities.Find(Guid.ParseExact(id, "D"));

            if (_speciality == null)
            {
                return NotFound();
            }

            _speciality.Name = speciality.Name;

            _context.Entry(_speciality).State = EntityState.Modified;

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

        // POST: api/Specialities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<string>> PostSpeciality(SpecialityPostDTO specialityDTO)
        {
            //var qual_id = Guid.ParseExact(specialityDTO.QualificationId, "D");
            //Qualification qual = _context.Qualifications.Find(qual_id);

            
            var speciality = new Speciality
            {
                Name = specialityDTO.Name,
                //QualificationId = qual.Id,
               
            };

            _context.Specialities.Add(speciality);
            await _context.SaveChangesAsync();

            return speciality.Id.ToString(); 
        }

        // DELETE: api/Specialities/5
        [HttpPost("delete")]
        public async Task<IActionResult> PostSpecialityDelete(List<string> ids)
        {
            var _guids = ids.Select(z => Guid.ParseExact(z, "D"));

            var _speciality = _context.Specialities.Where(x => _guids.Any(z => z == x.Id)).ToList();
           
            _context.Specialities.RemoveRange(_speciality);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SpecialityExists(Guid id)
        {
            return _context.Specialities.Any(e => e.Id == id);
        }
    }
}
