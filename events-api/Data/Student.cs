using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace events_api.Data
{
    public class Student
    {
       
            public Student()
            {

            }

            public Guid Id { get; set; }
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public DateTimeOffset DateOfBirth { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string Nationality { get; set; }
            public ICollection<Event> Events { get; set; }
        public List<Place> Places { get; set; }
        public Guid GroupId { get; set; }

        public Group Group { get; set; }
    }
}

