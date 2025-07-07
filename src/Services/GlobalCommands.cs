using NeoMUD.src;

namespace NeoMUD.src.Services;

public static class GlobalCommands
{
    public static async Task PING(GameSession session)
    {
      await session.SendTelnetStringAsync("PONG");
    }
}
