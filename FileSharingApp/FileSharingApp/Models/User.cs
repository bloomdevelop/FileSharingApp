using System.ComponentModel.DataAnnotations;

namespace FileSharingApp.Models;

public class User
{
    [Key]
    [MaxLength(50)]
    public string Username { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}
