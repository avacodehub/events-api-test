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


    public class SpecialityDTO
    {
        public SpecialityDTO() { }
        public SpecialityDTO(Speciality speciality)
        {
            this.Id = speciality.Id;
            this.Name = speciality.Name;

        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid QualificationId { get; set; }
        public string QualificationName { get; set; }


    }
    public class SpecialityPostDTO
    {
        
        public string Name { get; set; }
        
    }
    public class SpecialityFilterDTO
    {
        public string? SearchTerm { get; set; }
    }
}
