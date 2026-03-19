namespace Notes_App_C_.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty; // Nadpis [cite: 26]
        public string Content { get; set; } = string.Empty; // Text [cite: 26]
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Automatický čas [cite: 26]
        public bool IsImportant { get; set; } = false; // Příznak důležitosti [cite: 28]

        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
