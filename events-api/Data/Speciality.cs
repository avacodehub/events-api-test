using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace events_api.Data
{
    public class Speciality
    {
        public Speciality()
        {

        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        
        public List<Qualification> Qualifications { get; set; }
    }
}
