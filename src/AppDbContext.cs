using Microsoft.EntityFrameworkCore;
using NeoMUD.src;

public class AppDbContext : DbContext {
  
  public DbSet<Room> Rooms {get;set;} = default!;

  protected override void OnConfiguring(DbContextOptionsBuilder builder){
    builder.UseSqlite("data.db");
  }
}
