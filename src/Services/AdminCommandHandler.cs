using Microsoft.Extensions.Logging;
using NeoMUD.src.Services.Helpers;
using SuperSocket.ProtoBase;

namespace NeoMUD.src.Services;

public class AdminCommandHandler(GameSession session, ILogger<AdminCommandHandler> logger, AppDbContext db)
{
    public async Task ReceiveInput(StringPackageInfo pkg)
    {
      switch (pkg.Key.ToUpper()){
        case "#!USERS":
          await USERS();
          break;
        case "#!ROOMS":
          await ROOMS();
          break;
      }
    }

    public async ValueTask USERS()
    {
      foreach (var user in db.Users.ToList())
      {
        await session.Print(user.Username);
      }
    }

    public async ValueTask ROOMS()
    {
      foreach (var room in db.Rooms.ToList())
      {
        await session.Print($"{room.RoomName} ({room.Id})");
      }
    }

    public async ValueTask DEBUGINFO()
    {
      
    }
}
