using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace events_api.Data
{
    public class Qualification
    {
        public Qualification()
        {

        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public Guid SpecialityId { get; set; }

        public Speciality Speciality { get; set; }
        public List<Group> Groups { get; set; }

    }
}

