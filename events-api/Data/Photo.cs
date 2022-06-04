namespace events_api.Data
{
    public class Photo
    {
        public Photo()
        {

        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Data { get; set; }
        public Guid EventId { get; set; }
        public Event Event { get; set; }
        //public byte[] Image { get; set; }

    }
    public class PhotoDTOPost
    {
          public string Name { get; set; }
             public string Data { get; set; }
}
    public class PhotoDTOView
    {
        public string Id { get; set; }
        public string EventId { get; set; }
        public string Name { get; set; }
        public string Data { get; set; }
    }
}
