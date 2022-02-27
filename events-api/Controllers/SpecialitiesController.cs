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
        public async Task<IActionResult> PutSpeciality(Guid id, Speciality speciality)
        {
            if (id != speciality.Id)
            {
                return BadRequest();
            }

            _context.Entry(speciality).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpecialityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Specialities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Speciality>> PostSpeciality(Speciality speciality)
        {
            _context.Specialities.Add(speciality);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSpeciality", new { id = speciality.Id }, speciality);
        }

        // DELETE: api/Specialities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpeciality(Guid id)
        {
            var speciality = await _context.Specialities.FindAsync(id);
            if (speciality == null)
            {
                return NotFound();
            }

            _context.Specialities.Remove(speciality);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SpecialityExists(Guid id)
        {
            return _context.Specialities.Any(e => e.Id == id);
        }
    }
}
