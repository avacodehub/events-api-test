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
    public class StudentsController : ControllerBase
    {
        private readonly Context _context;

        public StudentsController(Context context)
        {
            _context = context;
            _context.Groups.Load();
        }

        /* { filter: {groupId:"", qualificationsAd:"", SpecialitiesId:"", yearId:""},
        pageNum:6, 
        SearchTerm: ""
        }   
        */
        // POST: api/Students/search
        [HttpPost("search")]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> SearchStudents(StudentFilterDTO studentFilterDTO)
        {
            //Console.WriteLine(studentFilterDTO.GroupIds[0]);
            
            IQueryable<Student> q = _context.Students.Include(x => x.Group);

            if (studentFilterDTO.GroupIds !=null && studentFilterDTO.GroupIds.Any())
            {
                var _guids = studentFilterDTO.GroupIds.Select(z => Guid.ParseExact(z, "D")).ToList();
                q = q.Where(x => _guids.Any(z => z == x.GroupId));
            }

            if (!String.IsNullOrEmpty(studentFilterDTO.SearchTerm))
            {
                q = q.Where(x => x.LastName.Contains(studentFilterDTO.SearchTerm));
            }
            return await q.Select(student => new StudentDTO(student)).ToListAsync();
        }
        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            return await _context.Students.ToListAsync();
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentGetDTO>> GetStudent(string id)
        {
            var student = await _context.Students.FindAsync(Guid.ParseExact(id, "D"));

            if (student == null)
            {
                return NotFound();
            }
            var studentGet = new StudentGetDTO
            {
                Id = student.Id.ToString(),
                LastName = student.LastName,
                FirstName = student.FirstName,
                MiddleName = student.MiddleName,
                Email = student.Email,
                Phone = student.Phone,
                Nationality = student.Nationality,
                GroupId = student.GroupId.ToString(),
                Birthdate = student.DateOfBirth ?? null,


            };
            return studentGet;
        }

        // PUT: api/Students/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(string id, StudentPostDTO student)
        {

            Student _student = _context.Students.Find(Guid.ParseExact(id, "D"));

            if (_student == null)
            {
                return NotFound();
            }

            _student.LastName = student.LastName;
            _student.FirstName = student.FirstName;
            _student.MiddleName = student.MiddleName;
            _student.Email = student.Email;
            _student.Phone = student.Phone;
            _student.Nationality = student.Nationality;
            _student.GroupId = student.GroupId;
            _student.DateOfBirth = student.Birthdate?.ToUniversalTime();
            _student.Group = _context.Groups.FirstOrDefault(x => x.Id == student.GroupId);

            _context.Entry(_student).State = EntityState.Modified;

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

        // POST: api/Students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<string>> PostStudent(StudentPostDTO studentDto)
        {
            var student = new Student
            {
                LastName = studentDto.LastName,
                FirstName = studentDto.FirstName,
                MiddleName = studentDto.MiddleName,
                Email = studentDto.Email,
                Phone = studentDto.Phone,
                Nationality = studentDto.Nationality,
                GroupId = studentDto.GroupId,
                DateOfBirth = studentDto.Birthdate?.ToUniversalTime(),
                Group = _context.Groups.FirstOrDefault(x => x.Id == studentDto.GroupId)

            };
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return student.Id.ToString();
        }

        // POST: api/students/Import
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("import")]
        public async Task<ActionResult<bool>> ImportStudents(List<StudentImportPostDTO> studentsImportDto)
        {
            var _students = studentsImportDto.Select(x =>
            {
                var _group = _context.Groups.FirstOrDefault(z => z.Name == x.GroupName);

                if (_group == null)
                {
                    _group = _context.Groups.First();
                }

                return new Student
                {
                    LastName = x.LastName,
                    FirstName = x.FirstName,
                    MiddleName = x.MiddleName,
                    Email = x.Email,
                    Phone = x.Phone,
                    Nationality = x.Nationality,
                    GroupId = _group.Id,
                    DateOfBirth = null,
                    Group = _group,
                    Places = new List<Place>()
                    
                };
            });

            _context.Students.AddRange(_students);
            await _context.SaveChangesAsync();

            return true;
        }

        // DELETE: api/Students/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteStudent(Guid id)
        //{
        //    var student = await _context.Students.FindAsync(id);
        //    if (student == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Students.Remove(student);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private bool StudentExists(Guid id)
        {
            return _context.Students.Any(e => e.Id == id);
        }

        // DELETE: api/group/5
        [HttpPost("delete")]
        public async Task<IActionResult> PostStudentDelete(List<string> ids)
        {
            var _guids = ids.Select(z => Guid.ParseExact(z, "D"));
            var _students = _context.Students.Include(x => x.Places).Where(x => _guids.Any(z => z == x.Id)).ToList();

            _context.Students.RemoveRange(_students);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpPost("exportStudents")]
        public async Task<ActionResult<IEnumerable<string[]>>> ExportStudents(string[] ids )
        {
            var _guids = ids.Select(z => Guid.ParseExact(z, "D"));
            var students = _context.Students.Include(x => x.Events).Where(x => _guids.Any(z => z == x.Id)).ToList();

            var events = new List<string[]>();
            foreach (var student in students)
            {
                bool firstEntry = true;
                foreach (var item in student.Events)
                {
                   
                    string fullname = firstEntry ? student.FirstName + " " + student.LastName + " " + student.MiddleName : "";
                    string[] _event = {
                        fullname,
                        item.Name,
                        item.DateStart.ToString("dd.MM.yyyy"),
                        item.Place,
                       
                         };
                    events.Add(_event);

                    firstEntry = false;
                }
            }

            return events;
        }
    }
}
