using FileSharingApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FileSharingApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<FileRecord> FileRecords { get; set; }
    public DbSet<User> Users { get; set; }
}
