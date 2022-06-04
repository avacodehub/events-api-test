namespace events_api.Dtos
{
    public class StudentGetDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        
        public DateTimeOffset DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Nationality { get; set; }
      
        public Guid GroupId { get; set; }
        public string GroupName { get; set; }

        public StudentGetDto()
        {
            string FullName;
            string Email;

        }

    }
}
