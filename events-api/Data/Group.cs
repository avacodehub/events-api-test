using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace events_api.Data
{
    public class Group
    {
        public Group()
        {

        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset Date { get; set; }

        public Guid QualificationId { get; set; }

        public Qualification Qualification { get; set; }
        public ICollection<Event> Events { get; set; }
        public List<Student> Students { get; set; }

    }

    public class GroupDTO
    {
        public GroupDTO() { }
        public GroupDTO(Group group)
        {
            this.Id = group.Id;
            this.Name = group.Name;
            this.QualificationId = group.QualificationId;
            this.QualificationName = group.Qualification.Name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid QualificationId { get; set; }
        public string QualificationName { get; set; }


    }
    public class GroupPostDTO
    {

        public string Name { get; set; }

        public DateTimeOffset Date { get; set; }

        public string QualificationId { get; set; }



    }
    public class GroupPutDTO
    {

        public string Name { get; set; }

        public DateTimeOffset Date { get; set; }

        public Guid QualificationId { get; set; }



    }
    public class GroupGetDTO
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTimeOffset Date { get; set; }

        public string QualificationId { get; set; }

    }

    /// <summary>
    /// //////////////было 
    /// </summary>
    //public partial class GroupPostDTO2
    //{
    //    public GroupPostDTO()
    //    {
    //    }

    //    public string Name { get; set; }
    //    public DateTimeOffset Date { get; set; }
    //}
    public partial class GroupListDTO
    {
        public GroupListDTO()
        {
        }
        public Guid Id { get; set; }

        public string Name { get; set; }
        public DateTimeOffset Date { get; set; }

        public GroupListDTO(Group group)
        {
            this.Id = group.Id;
            this.Name = group.Name;
            this.Date = group.Date;
        }

    }

    public class GroupFilterDTO
    {
        public List<string>? QualificationIds { get; set; }
        public string? SearchTerm { get; set; }
    }

}
