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

        public string? DocsLink { get; set; }
        public string? PhotosLink { get; set; }
    }
}

