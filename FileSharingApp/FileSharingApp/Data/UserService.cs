using FileSharingApp.Data;
using FileSharingApp.Models;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;

namespace FileSharingApp.Data;

public class UserService
{
    private readonly IDbContextFactory<AppDbContext> _dbFactory;
    private readonly ProtectedLocalStorage _localStorage;
    
    public string? CurrentUsername { get; private set; }
    public event Action? OnChange;

    public UserService(IDbContextFactory<AppDbContext> dbFactory, ProtectedLocalStorage localStorage)
    {
        _dbFactory = dbFactory;
        _localStorage = localStorage;
    }

    public async Task InitializeAsync()
    {
        var result = await _localStorage.GetAsync<string>("username");
        if (result.Success)
        {
            CurrentUsername = result.Value;
            NotifyStateChanged();
        }
    }

    public async Task<bool> LoginAsync(string username)
    {
        if (string.IsNullOrWhiteSpace(username)) return false;
        username = username.Trim();

        using var db = await _dbFactory.CreateDbContextAsync();
        var user = await db.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
        
        if (user != null)
        {
            CurrentUsername = user.Username; // Use the stored casing
            await _localStorage.SetAsync("username", CurrentUsername);
            NotifyStateChanged();
            return true;
        }
        
        return false;
    }

    public async Task<bool> SignupAsync(string username)
    {
        if (string.IsNullOrWhiteSpace(username)) return false;
        username = username.Trim();

        using var db = await _dbFactory.CreateDbContextAsync();
        if (await db.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower()))
        {
            return false;
        }

        db.Users.Add(new User { Username = username });
        await db.SaveChangesAsync();
        
        CurrentUsername = username;
        await _localStorage.SetAsync("username", username);
        NotifyStateChanged();
        return true;
    }

    public async Task LogoutAsync()
    {
        CurrentUsername = null;
        await _localStorage.DeleteAsync("username");
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
