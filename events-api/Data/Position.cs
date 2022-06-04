using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace events_api.Data
{
    public class Position
    {
        public Position()
        {

        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Employee> Employees { get; set; }
    }
    public class PositionDTO
    {
        public PositionDTO() { }
        public PositionDTO(Position position)
        {
            this.Id = position.Id;
            this.Name = position.Name;
           
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
      


    }
}
