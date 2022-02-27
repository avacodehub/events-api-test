using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace events_api.Data
{
    public class Employee
    {

        public Employee()
        {

        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Phone { get; set; }
        public Guid PositionId { get; set; }
        public Position Position { get; set; }
        public ICollection<Event> Events { get; set; }

    }
}
