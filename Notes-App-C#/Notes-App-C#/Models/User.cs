using System.ComponentModel.DataAnnotations;

namespace Notes_App_C_.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; } = string.Empty; // Musí být unikátní [cite: 23]
        [Required]
        public string PasswordHash { get; set; } = string.Empty; // Pouze hash [cite: 23]
        public List<Note> Notes { get; set; } = new();
    }
}
