using NeoMUD.src;
using NeoMUD.src.Services.Helpers;

namespace NeoMUD.src.Services;

public static class GlobalCommandHandler
{
    public static async Task PING(GameSession session)
    {
      await session.Print("PONG");
    }
}
