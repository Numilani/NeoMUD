using SuperSocket.Command;
using SuperSocket.ProtoBase;

namespace NeoMUD.src.commands;

public class PING : IAsyncCommand<GameSession, StringPackageInfo>
{
  public async ValueTask ExecuteAsync(GameSession session, StringPackageInfo package, CancellationToken cancellationToken)
  {
    await session.SendTelnetStringAsync("PONG");
    // await session.SendAsync(Encoding.UTF8.GetBytes("PONG" + "\r\n"));
  }
}
