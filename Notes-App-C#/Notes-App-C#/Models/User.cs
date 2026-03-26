using System.ComponentModel.DataAnnotations;

namespace Notes_App_C_.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress] // Validace formátu emailu
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public List<Note> Notes { get; set; } = new();
    }
}
