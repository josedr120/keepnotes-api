namespace keepnotes_api.DTOs
{
    public class NoteDto
    {
        public string Id { get; set; }
        
        public string UserId { get; set; }
        
        public string Title { get; set; }
        
        public string Content { get; set; }
    }
}