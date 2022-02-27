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

    public partial class GroupPostDTO
    {
        public GroupPostDTO()
        {
        }

        public string Name { get; set; }
        public DateTimeOffset Date { get; set; }
    }
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


    public partial class GroupDTO
    {
        public GroupDTO()
        {
        }
        public Guid Id { get; set; }

        public string Name { get; set; }
        public DateTimeOffset Date { get; set; }

        public GroupDTO(Group group)
        {
            this.Id = group.Id;
            this.Name = group.Name;
            this.Date = group.Date;
        }

    }
}
