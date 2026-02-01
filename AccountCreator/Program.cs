using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

var services = new ServiceCollection();
services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

var serviceProvider = services.BuildServiceProvider();
using var context = serviceProvider.GetRequiredService<AppDbContext>();

Console.WriteLine("=== Account Creator ===");
Console.Write("Username: ");
var username = Console.ReadLine();

Console.Write("Password: ");
var password = Console.ReadLine();

Console.Write("Name: ");
var name = Console.ReadLine();

Console.Write("Email: ");
var email = Console.ReadLine();

Console.WriteLine("Role:");
Console.WriteLine("1. SuperAdmin");
Console.WriteLine("2. Admin");
Console.WriteLine("3. User");
Console.Write("Select role (1-3): ");
var roleInput = Console.ReadLine();

if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || 
    string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) ||
    !int.TryParse(roleInput, out var roleId) || roleId < 1 || roleId > 3)
{
    Console.WriteLine("Invalid input!");
    return;
}

// Check if username exists
var existingUser = await context.Users.FirstOrDefaultAsync(u => u.Username == username);
if (existingUser != null)
{
    Console.WriteLine("Username already exists!");
    return;
}

// Generate salt and hash password
var salt = GenerateSalt();
var hashedPassword = HashPassword(password, salt);

// Create user profile
var userProfile = new UserProfile
{
    Name = name,
    Email = email
};

context.UserProfiles.Add(userProfile);
await context.SaveChangesAsync();

// Create user
var user = new User
{
    Username = username,
    Password = hashedPassword,
    PasswordSalt = salt,
    RoleId = roleId,
    UserProfileId = userProfile.Id,
    IsActivated = true
};

context.Users.Add(user);
await context.SaveChangesAsync();

Console.WriteLine($"Account created successfully! User ID: {user.Id}");

static string GenerateSalt()
{
    var salt = RandomNumberGenerator.GetBytes(16);
    return Convert.ToBase64String(salt);
}

static string HashPassword(string password, string salt)
{
    var saltBytes = Convert.FromBase64String(salt);
    var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
    {
        Salt = saltBytes,
        DegreeOfParallelism = 1,
        Iterations = 2,
        MemorySize = 1024 * 64
    };
    var hash = argon2.GetBytes(16);
    return Convert.ToBase64String(saltBytes.Concat(hash).ToArray());
}

public class User
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;
    [MaxLength(255)]
    public string Password { get; set; } = string.Empty;
    [MaxLength(255)]
    public string PasswordSalt { get; set; } = string.Empty;
    public int RoleId { get; set; }
    public Guid UserProfileId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActivated { get; set; }
}

public class UserProfile
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;
    [MaxLength(20)]
    public string? Phone { get; set; }
    [MaxLength(500)]
    public string? Address { get; set; }
    [MaxLength(100)]
    public string? City { get; set; }
    [MaxLength(255)]
    public string? ProfilePicture { get; set; }
}

public class Role
{
    public int Id { get; set; }
    [MaxLength(50)]
    public string? Name { get; set; }
    public int Level { get; set; }
}

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasData(
                new Role { Id = 1, Name = "SuperAdmin", Level = 1 },
                new Role { Id = 2, Name = "Admin", Level = 2 },
                new Role { Id = 3, Name = "User", Level = 3 }
            );
        });

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(e => e.Id);
        });
    }
}