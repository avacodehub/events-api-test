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

    public class QualificationDTO
    {
        public QualificationDTO() { }
        public QualificationDTO(Qualification qualification)
        {
            this.Id = qualification.Id;
            this.Name = qualification.Name;
            this.SpecialityId = qualification.SpecialityId;
            this.SpecialityName = qualification.Speciality.Name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid SpecialityId { get; set; }
        public string SpecialityName { get; set; }


    }
    public class QualificationPostDTO
    {

        public string Name { get; set; }
        public string SpecialityId { get; set; }

    }
    public class QualificationPutDTO
    {
        public string Name { get; set; }
        public Guid SpecialityId { get; set; }

    }
    public class QualificationFilterDTO
    {
        public List<string>? SpecialityIds { get; set; }
        public string? SearchTerm { get; set; }

    }
}

