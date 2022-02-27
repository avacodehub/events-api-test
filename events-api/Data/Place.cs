
namespace events_api.Data
{
    public class Place
    {
        public Place()
        {

        }

        public Guid Id { get; set; }
        public string Number { get; set; }
        public Guid StudentId { get; set; }

        public Student Student { get; set; }
        public Guid EventId { get; set; }

        public Event Event { get; set; }

    }
}
