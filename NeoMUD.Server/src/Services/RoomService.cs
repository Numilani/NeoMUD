using Microsoft.Extensions.Logging;
using NeoMUD.src.Models;

namespace NeoMUD.src.Services;

public class RoomService(AppDbContext _context, ILogger<RoomService> _logger)
{

  public async Task<Room?> GetRoom(string id)
  {
    return await _context.Rooms.FindAsync(id);
  }

  public async Task CreateRoomAsync(Room room)
  {
    try
    {
      _context.Rooms.Update(room);
      await _context.SaveChangesAsync();
    }
    catch (Exception e)
    {
      _logger.LogWarning(e, "Failed to create room");
    }
  }

}
