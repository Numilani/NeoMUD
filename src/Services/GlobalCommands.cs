using NeoMUD.src;
using NeoMUD.src.Services.Helpers;

namespace NeoMUD.src.Services;

public static class GlobalCommands
{
    public static async Task PING(GameSession session)
    {
      await session.PrintLine("PONG");
    }
}
