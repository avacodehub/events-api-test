namespace events_api.Data
{
    public class Document
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Data { get; set; }
        public Guid EventId { get; set; }
        public Event Event { get; set; }
     

    }
    public class DocumentDTOPost
    {
        public string Name { get; set; }
        public string Data { get; set; }
    }
    public class DocumentDTOView
    {
        public string Id { get; set; }
        public string EventId { get; set; }
        public string Name { get; set; }
        public string Data { get; set; }
    }
}

