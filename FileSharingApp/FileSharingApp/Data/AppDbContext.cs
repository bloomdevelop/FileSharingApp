using FileSharingApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FileSharingApp.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<FileRecord> FileRecords { get; set; }
    public DbSet<User> Users { get; set; }
}
