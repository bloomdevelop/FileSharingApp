using System.ComponentModel.DataAnnotations;

namespace FileSharingApp.Models;

public class FileRecord
{
    [Key]
    public int Id { get; init; }
    [MaxLength(32)]
    public string ShareId { get; init; } = Guid.NewGuid().ToString("n");
    [MaxLength(255)]
    public string FileName { get; init; } = string.Empty;
    [MaxLength(50)]
    public string Username { get; init; } = string.Empty;
    public DateTime UploadDate { get; init; } = DateTime.UtcNow;
}
