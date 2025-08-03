using Microsoft.EntityFrameworkCore;
using NeoMUD.src.Models;

namespace NeoMUD.src;

public class AppDbContext : DbContext
{
  public AppDbContext(DbContextOptions options) : base(options)
  {
  }

  protected AppDbContext()
  {
  }

  public DbSet<User> Users { get; set; } = default!;
  public DbSet<Character> Characters { get; set; } = default!;
  public DbSet<Room> Rooms { get; set; } = default!;

  protected override void OnConfiguring(DbContextOptionsBuilder builder)
  {
  }
}
