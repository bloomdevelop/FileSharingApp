using System.Security.Cryptography;
using FileSharingApp.Models;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;

namespace FileSharingApp.Data;

public class UserService(IDbContextFactory<AppDbContext> dbFactory, ProtectedLocalStorage localStorage)
{
    public string? CurrentUsername { get; private set; }
    public event Action? OnChange;

    public async Task InitializeAsync()
    {
        try
        {
            var result = await localStorage.GetAsync<string>("username");
            if (result.Success)
            {
                CurrentUsername = result.Value;
                NotifyStateChanged();
            }
        }
        catch (CryptographicException)
        {
            await LogoutAsync();
        }
    }

    public async Task<bool> LoginAsync(string username)
    {
        if (string.IsNullOrWhiteSpace(username)) return false;
        username = username.Trim();

        await using var db = await dbFactory.CreateDbContextAsync();
        var user = await db.Users.FirstOrDefaultAsync(u => string.Equals(u.Username.ToLower(), username.ToLower(), StringComparison.OrdinalIgnoreCase));

        if (user == null) return false;
        
        CurrentUsername = user.Username; // Use the stored casing
        await localStorage.SetAsync("username", CurrentUsername);
        NotifyStateChanged();
        return true;

    }

    public async Task<bool> SignupAsync(string username)
    {
        if (string.IsNullOrWhiteSpace(username)) return false;
        username = username.Trim();

        await using var db = await dbFactory.CreateDbContextAsync();
        
        if (await db.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower()))
        {
            return false;
        }

        db.Users.Add(new User { Username = username });
        await db.SaveChangesAsync();

        CurrentUsername = username;
        await localStorage.SetAsync("username", username);
        NotifyStateChanged();
        return true;
    }

    public async Task LogoutAsync()
    {
        CurrentUsername = null;
        await localStorage.DeleteAsync("username");
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}