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
    public class QualificationsController : ControllerBase
    {
        private readonly Context _context;

        public QualificationsController(Context context)
        {
            _context = context;
            _context.Groups.Load();
        }

        [HttpPost("search")]
        public async Task<ActionResult<IEnumerable<QualificationDTO>>> SearchQualifications(QualificationFilterDTO qualificationFilterDTO)
        {
            
            IQueryable<Qualification> q = _context.Qualifications.Include(x => x.Speciality);

            if (qualificationFilterDTO.SpecialityIds!=null && qualificationFilterDTO.SpecialityIds.Any()  )
            {
                var _guids = qualificationFilterDTO.SpecialityIds.Select(z => Guid.ParseExact(z, "D")).ToList();
                q = q.Where(x => _guids.Any(z => z == x.SpecialityId));
            }

            if (!String.IsNullOrEmpty(qualificationFilterDTO.SearchTerm))
            {
                q = q.Where(x => x.Name.Contains(qualificationFilterDTO.SearchTerm));
            }
            return await q.Select(qualification => new QualificationDTO(qualification)).ToListAsync();

        }


        // GET: api/Qualifications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Qualification>>> GetQualifications()
        {
            return await _context.Qualifications.ToListAsync();
        }

        // GET: api/Qualifications/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Qualification>> GetQualification(Guid id)
        {
            var qualification = await _context.Qualifications.FindAsync(id);

            if (qualification == null)
            {
                return NotFound();
            }

            return qualification;
        }

        // PUT: api/Qualifications/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQualification(string id, QualificationPutDTO qualification)
        {

            Qualification _qualification = _context.Qualifications.Find(Guid.ParseExact(id, "D"));
           
            if (_qualification == null)
            {
                return NotFound();
            }
            _qualification.Name = qualification.Name;
            _qualification.SpecialityId = qualification.SpecialityId;
                
            _context.Entry(_qualification).State = EntityState.Modified;

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

        // POST: api/Qualifications
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<string>> PostQualification(QualificationPostDTO qualificationDTO)
        {
            var qual_id = Guid.ParseExact(qualificationDTO.SpecialityId, "D");
            Speciality qual = _context.Specialities.Find(qual_id);

            if (qual == null)
            {
                return NotFound();
            }
            var qualification = new Qualification
            {
                Name = qualificationDTO.Name,
                SpecialityId = qual.Id,
                

            };


            _context.Qualifications.Add(qualification);
            await _context.SaveChangesAsync();

            return qualification.Id.ToString();
        }

        // DELETE: api/Qualifications/5
        [HttpPost("delete")]
        public async Task<IActionResult> PostQualificationDelete(List<string> ids)
        {
            var _guids = ids.Select(z => Guid.ParseExact(z, "D"));
            var _qualification = _context.Qualifications.Where(x => _guids.Any(z => z == x.Id)).ToList();

            _context.Qualifications.RemoveRange(_qualification);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QualificationExists(Guid id)
        {
            return _context.Qualifications.Any(e => e.Id == id);
        }
    }
}
