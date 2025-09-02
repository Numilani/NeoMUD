using Microsoft.Extensions.Logging;
using NeoMUD.src.Services.Helpers;
using SuperSocket.ProtoBase;

namespace NeoMUD.src.Services;

public class AdminCommandHandler(IGameSession session, ILogger<AdminCommandHandler> logger, AppDbContext db)
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
        await session.FormMessage(user.Username).Send();
      }
    }

    public async ValueTask ROOMS()
    {
      foreach (var room in db.Rooms.ToList())
      {
        await session.FormMessage($"{room.RoomName} ({room.Id})").Send();
      }
    }

    public async ValueTask DEBUGINFO()
    {
      
    }
}
