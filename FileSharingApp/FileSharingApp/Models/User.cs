using System.ComponentModel.DataAnnotations;

namespace FileSharingApp.Models;

public class User
{
    [Key]
    public string Username { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
