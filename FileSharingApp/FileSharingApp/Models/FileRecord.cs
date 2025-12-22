using System.ComponentModel.DataAnnotations;

namespace FileSharingApp.Models;

public class FileRecord
{
    [Key]
    public int Id { get; set; }
    public string ShareId { get; set; } = Guid.NewGuid().ToString("n");
    public string FileName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public DateTime UploadDate { get; set; } = DateTime.UtcNow;
}
