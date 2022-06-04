using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace events_api.Data
{
    public class Event
    {
        public Event()
        {

        }

        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        public string Description { get; set; }
        public DateTimeOffset DateStart { get; set; }
        public DateTimeOffset DateEnd { get; set; }
        public ICollection<Student> Students { get; set; }
        public List<Place> Places { get; set; }
        public ICollection<Group> Groups { get; set; }
        public ICollection<Employee> Employees { get; set; }
        public List<Photo>? Photos { get; set; }
        public List<Document>? Documents { get; set; }
    }

    public class EventDTO
    {
        public EventDTO(Event events)
        {
            this.Id = events.Id;
            this.Name = events.Name;
            this.DateStart = events.DateStart;
            this.DateEnd = events.DateEnd;
            
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset DateStart { get; set; }
        public DateTimeOffset DateEnd { get; set; }

    }
    public class EventPostDTO
    {    
        public string Type { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        public string Description { get; set; }
        public DateTimeOffset DateStart { get; set; }
        public DateTimeOffset DateEnd { get; set; }
        public List<string> StudentIds  { get; set; }
        public List<string> GroupIds{ get; set; }
        public List<string> EmployeeIds { get; set; }
        public List<PhotoDTOPost>? Photos { get; set; }
        public List<DocumentDTOPost>? Documents { get; set; }

    }
    public class EventGetDTO
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        public string Description { get; set; }
        public DateTimeOffset DateStart { get; set; }
        public DateTimeOffset DateEnd { get; set; }
        public List<string> StudentIds { get; set; }
       
        public List<string> GroupIds { get; set; }
        public List<string> EmployeeIds { get; set; }
        

    }

    public class EventPutDTO
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        public string Description { get; set; }
        public DateTimeOffset DateStart { get; set; }
        public DateTimeOffset DateEnd { get; set; }
        public Guid StudentId { get; set; }
        public Guid GroupId { get; set; }
        public Guid EmployeeId { get; set; }
        

    }

    public class EventStudentsCountDTO
    {
 

        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset DateStart { get; set; }
        public int StudentCount { get; set; }

    }

    public class EventExportDTO
    {
        public Guid Id { get; set; }
        public string StudentName { get; set; }
        public string Name { get; set; }
        public DateTimeOffset DateStart { get; set; }
        public string Place { get; set; }
        
        public string Rank { get; set; }
    }

    public class EventFilterDTO
    {
        public List<string>? Type { get; set; }
        public string? SearchTerm { get; set; }
        public DateTimeOffset? DateStart { get; set; }
        public DateTimeOffset? DateEnd { get; set; }
        public List<string>? Places { get; set; }
    }


    public class EventStudentDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Rank { get; set; }
    }
    public class EventViewDTO
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        public string Description { get; set; }
        public DateTimeOffset DateStart { get; set; }
        public DateTimeOffset DateEnd { get; set; }
        public List<EventStudentDTO> Students { get; set; }
        public List<string> GroupNames { get; set; }
        public List<string> EmployeeNames { get; set; }

        public List<PhotoDTOView>? Photos { get; set; }
        public List<DocumentDTOView>? Documents { get; set; }
    }

}

