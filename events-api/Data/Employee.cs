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
      
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string Phone { get; set; }
        public Guid PositionId { get; set; }
        public Position Position { get; set; }
        public ICollection<Event> Events { get; set; }

    }
    public class EmployeeDTO
    {
        public EmployeeDTO(Employee employee)
        {
            this.Id = employee.Id;
            this.FullName = employee.LastName +" "+ employee.FirstName + " "+ employee.MiddleName;
            this.PositionId = employee.PositionId;
            this.PositionName = employee.Position.Name;
        }

        public Guid Id { get; set; }
        public string FullName { get; set; }
        public Guid PositionId { get; set; }
        public string PositionName { get; set; }
       

    }

    public class EmployeePostDTO
    {

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
     
        public string Phone { get; set; }
        public Guid PositionId { get; set; }


    }
    public class EmployeeFilterDTO
    {
        public List<string>? PositionIds { get; set; }
        public string? SearchTerm { get; set; }

    }
}
