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
    public class EventsController : ControllerBase
    {
        private readonly Context _context;

        public EventsController(Context context)
        {
            _context = context;
            _context.Event.Load();
            _context.Students.Load();
            _context.Groups.Load();
            _context.Employees.Load();
        }


        [HttpPost("search")]
        public async Task<ActionResult<IEnumerable<EventDTO>>> SearchEvent(EventFilterDTO eventFilterDTO)
        {
            IQueryable<Event> q = _context.Event;

            if (eventFilterDTO.Type.Any())
            {
                q = q.Where(x => eventFilterDTO.Type.Any(z => z == x.Type));
            }

            if (!String.IsNullOrEmpty(eventFilterDTO.SearchTerm))
            {
                q = q.Where(x => x.Name.Contains(eventFilterDTO.SearchTerm));
            }
            if (eventFilterDTO.DateStart != null)
            {
                q = q.Where(x => x.DateStart > eventFilterDTO.DateStart);
            }
            if (eventFilterDTO.DateEnd != null)
            {
                q = q.Where(x => x.DateStart < eventFilterDTO.DateEnd);
            }
            if (eventFilterDTO.Places != null && eventFilterDTO.Places.Any())
            {
                // q = q.Where(x => eventFilterDTO.Places.Any(z => z == x.));
                q = q.Where(x => x.Places.Any(z => eventFilterDTO.Places.IndexOf(z.Number) >= 0));
            }
            return await q.Select(_event => new EventDTO(_event)).ToListAsync();

        }



        // GET: api/Events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventStudentsCountDTO>>> GetEvent()
        {

            return await _context.Event.Select(x => new EventStudentsCountDTO
            {
                Id = x.Id,
                Name = x.Name,
                DateStart = x.DateStart,
                StudentCount = x.Students.Count
            }).ToListAsync();
        }

        // GET: api/Events/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventGetDTO>> GetEvent(string id)
        {

            var eventItem = _context.Event
               .Include(x => x.Students)
               .Include(x => x.Groups).Include(x => x.Employees).FirstOrDefault(x => x.Id == Guid.ParseExact(id, "D"));

            if (eventItem == null)
            {
                return NotFound();
            }


            var eventGet = new EventGetDTO
            {
                Id = eventItem.Id.ToString(),
                Type = eventItem.Type,
                Name = eventItem.Name,
                Place = eventItem.Place,
                DateStart = eventItem.DateStart,
                DateEnd = eventItem.DateEnd,
                Description = eventItem.Description,
                //Students = _students,
                GroupIds = eventItem.Groups.Select(x => x.Id.ToString()).ToList(),
                StudentIds = eventItem.Students.Select(x => x.Id.ToString()).ToList(),
                EmployeeIds = eventItem.Employees.Select(x => x.Id.ToString()).ToList(),
            };
            return eventGet;

        }

        // GET: api/Events/5/ranks
        [HttpGet("{id}/ranks")]
        public async Task<ActionResult<List<EventStudentDTO>>> GetEventRanks(string id)
        {

            var eventItem = _context.Event
                .Include(x => x.Students)
               .Include(x => x.Places).FirstOrDefault(x => x.Id == Guid.ParseExact(id, "D"));

            if (eventItem == null)
            {
                return NotFound();
            }

            List<EventStudentDTO> _students = eventItem.Students.Select(x => new EventStudentDTO
            {
                Id = x.Id.ToString(),
                Name = x.FirstName + " " + x.LastName + " " + x.MiddleName,
                Rank = eventItem.Places.FirstOrDefault(z => z.StudentId == x.Id)?.Number
            }).ToList();


            return _students;
        }

        // GET: api/Events/5
        [HttpGet("view/{id}")]
        public async Task<ActionResult<EventViewDTO>> GetEventView(string id)
        {


            var eventItem = _context.Event
                .Include(x => x.Students)
                .Include(x => x.Groups)
                .Include(x => x.Employees)
                .Include(x => x.Photos)
                .Include(x => x.Documents)
                .Include(x => x.Places).FirstOrDefault(x => x.Id == Guid.ParseExact(id, "D"));

            if (eventItem == null)
            {
                return NotFound();
            }

            List<EventStudentDTO> _students = eventItem.Students.Select(x => new EventStudentDTO
            {
                Id = x.Id.ToString(),
                Name = x.FirstName + " " + x.LastName + " " + x.MiddleName,
                Rank = eventItem.Places.FirstOrDefault(z => z.StudentId == x.Id)?.Number
            }).ToList();

            var eventGet = new EventViewDTO
            {
                Id = eventItem.Id.ToString(),
                Type = eventItem.Type,
                Name = eventItem.Name,
                Place = eventItem.Place,
                DateStart = eventItem.DateStart,
                DateEnd = eventItem.DateEnd,
                Description = eventItem.Description,
                GroupNames = eventItem.Groups.Select(x => x.Name).ToList(),
                Students = _students,
                EmployeeNames = eventItem.Employees.Select(x => x.FirstName + " " + x.LastName + " " + x.MiddleName).ToList(),
                Photos = eventItem.Photos.Select(x => new PhotoDTOView { Id=x.Id.ToString(), Data=x.Data, Name=x.Name, EventId=x.EventId.ToString() }).ToList(),
                Documents = eventItem.Documents.Select(x => new DocumentDTOView { Id = x.Id.ToString(), Data = x.Data, Name = x.Name, EventId = x.EventId.ToString() }).ToList(),
            };
            return eventGet;
        }

        // PUT: api/Events/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(string id, EventPostDTO eventItem)
        {
            var _guid = Guid.ParseExact(id, "D");
            var _event = _context.Event.Include(x => x.Groups).Include(x => x.Students)
                .Include(x => x.Employees).FirstOrDefault(x => x.Id == _guid);

            if (_event == null)
            {
                return NotFound();
            }

            var _groupGuids = eventItem.GroupIds.Select(x => Guid.ParseExact(x, "D"));
            var _employeeGuids = eventItem.EmployeeIds.Select(x => Guid.ParseExact(x, "D"));
            var _studentsGuids = eventItem.StudentIds.Select(x => Guid.ParseExact(x, "D"));

            var _eventEmployees = _event.Employees;
            var _employees = _context.Employees.Where(x => _groupGuids.Any(z => x.Id == z));

            var newEmployees = new List<Employee>();

            //foreach(var employee in _eventEmployees)
            //{
            //    if(_employeeGuids.Any(x => x== employee.Id))
            //    {
            //        newEmployees.Add(employee);
            //    }
            //}

            //foreach (var guid in _employeeGuids)
            //{
            //    if (newEmployees.Any(x => x.Id == guid))
            //    {
            //        continue;
            //    }
            //    else
            //    {
            //        var employee = _context.Employees.FirstOrDefault(x => x.Id == guid);
            //        newEmployees.Add(employee);
            //    }
            //}

            _event.Name = eventItem.Name;
            _event.DateStart = eventItem.DateStart.ToUniversalTime();
            _event.DateEnd = eventItem.DateEnd.ToUniversalTime();
            _event.Description = eventItem.Description;
            _event.Place = eventItem.Place;
            _event.Type = eventItem.Type;   

            _event.Groups = _context.Groups.Where(x => _groupGuids.Any(z => z == x.Id)).ToList();
            //_event.Employees = newEmployees;
            _event.Employees = _context.Employees.Where(x => _employeeGuids.Any(z => z == x.Id)).ToList();
            _event.Students = _context.Students.Where(x => _studentsGuids.Any(z => z == x.Id)).ToList();

            _context.Entry(_event).State = EntityState.Modified;

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
        [HttpPost("exportEvents")]
        public async Task<ActionResult<IEnumerable<string[]>>> ExportEvents(string[] ids)
        {
            var _guids = ids.Select(z => Guid.ParseExact(z, "D"));

            var events = _context.Event.Include(x => x.Students).Include(x => x.Places).Where(x => _guids.Any(z => z == x.Id)).ToList();

            var students = new List<string[]>();
            foreach (var item in events)
            {

                foreach (var student in item.Students)
                {
                    string fullname = student.FirstName + " " + student.LastName + " " + student.MiddleName;
                    var place = item.Places?.FirstOrDefault(x => x.StudentId == student.Id);
                    string placeValue = place != null ? place.Number : "-";
                    string[] _event = {
                        item.Name,
                        fullname,
                        item.DateStart.ToString("dd.MM.yyyy"),
                        item.Place,

                        placeValue };
                    students.Add(_event);
                }
            }

            return students;
        }

        // DELETE: api/event/5
        [HttpPost("delete")]
        public async Task<IActionResult> PostEventsDelete(List<string> ids)
        {
            var _guids = ids.Select(z => Guid.ParseExact(z, "D"));
            var _event = _context.Event.Include(x => x.Places).Include(x => x.Photos).Include(x => x.Documents).Where(x => _guids.Any(z => z == x.Id)).ToList();

            _context.Event.RemoveRange(_event);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventExists(Guid id)
        {
            return _context.Event.Any(e => e.Id == id);
        }

        [HttpPost]
        public async Task<ActionResult<string>> PostEvent(EventPostDTO eventDto)
        {
            var _groupGuids = eventDto.GroupIds.Select(x => Guid.ParseExact(x, "D"));
            var _employeeGuids = eventDto.EmployeeIds.Select(x => Guid.ParseExact(x, "D"));
            var _studentsGuids = eventDto.StudentIds.Select(x => Guid.ParseExact(x, "D"));
            var _photos = eventDto.Photos.Select(x => new Photo { Name = x.Name, Data = x.Data }).ToList();
            var _documents = eventDto.Documents.Select(x => new Document { Name = x.Name, Data = x.Data }).ToList();
            var events = new Event
            {
                Type = eventDto.Type,
                Name = eventDto.Name,
                Place = eventDto.Place,
                Description = eventDto.Description,
                DateStart = eventDto.DateStart.ToUniversalTime(),
                DateEnd = eventDto.DateEnd.ToUniversalTime(),
                Photos = _photos,
                Documents = _documents,

                Groups = _context.Groups.Where(x => _groupGuids.Any(z => z == x.Id)).ToList(),
                Employees = _context.Employees.Where(x => _employeeGuids.Any(z => z == x.Id)).ToList(),
                Students = _context.Students.Where(x => _studentsGuids.Any(z => z == x.Id)).ToList(),

            };
            _context.Event.Add(events);
            await _context.SaveChangesAsync();

            return events.Id.ToString();
        }

        // PUT: api/Events/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("ranks/{id}")]
        public async Task<IActionResult> PutEventRanks(string id, List<EventStudentDTO> ranks)
        {
            var _guid = Guid.ParseExact(id, "D");
            var _event = _context.Event.Include(x => x.Places).Include(x => x.Students).FirstOrDefault(x => x.Id == _guid);
            if (_event == null)
            {
                return NotFound();
            }
            var _places = _event.Places;

            foreach (var x in ranks)
            {
                var _studentGuid = Guid.ParseExact(x.Id, "D");
                var _place = _places.FirstOrDefault(p => p.StudentId == _studentGuid);
                if (_place != null)
                {
                    _place.Number = x.Rank;
                }
                else
                {
                    var _student = _event.Students.FirstOrDefault(s => s.Id == _studentGuid);
                    _places.Add(new Place { Number = x.Rank, Event = _event, Student = _student });
                }
            }
            _context.Entry(_event).State = EntityState.Modified;

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
