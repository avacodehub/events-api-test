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
        public string? MiddleName { get; set; }
        public DateTimeOffset? DateOfBirth { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Nationality { get; set; }
        public ICollection<Event> Events { get; set; }
        public List<Place> Places { get; set; }
        public Guid GroupId { get; set; }
        public Group Group { get; set; }
    }

    public class StudentDTO
    {
        public StudentDTO(Student student)
        {
            this.Id = student.Id;
            this.FullName = student.LastName + " " + student.FirstName + " " + student.MiddleName;
            this.Email = student.Email;
            this.GroupId = student.GroupId;
            this.GroupName = student.Group.Name;
        }

        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public Guid GroupId { get; set; }
        public string GroupName { get; set; }

    }
    public class StudentPostDTO
    {

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public DateTimeOffset? Birthdate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Nationality { get; set; }

        public Guid GroupId { get; set; }

    }

    public class StudentImportPostDTO
    {

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Nationality { get; set; }

        public string GroupName { get; set; }

    }


    public class StudentGetDTO
    {
        public string Id { get; set; }

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public DateTimeOffset? Birthdate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Nationality { get; set; }

        public string GroupId { get; set; }

    }
    public class StudentFilterDTO
    {
        public List<string>? GroupIds { get; set; }
        public string? SearchTerm { get; set; }
    }
}