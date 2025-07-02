using SuperSocket.Command;
using SuperSocket.ProtoBase;

namespace NeoMUD.src.commands;

public class LOGIN : IAsyncCommand<GameSession, StringPackageInfo>
{
    public async ValueTask ExecuteAsync(GameSession session, StringPackageInfo package, CancellationToken cancellationToken)
    {
      if (package.Parameters.Length != 2){
        await session.SendTelnetStringAsync("Syntax: LOGIN <username> <password>");
        return;
      }

      var username = package.Parameters[0];
      var password = package.Parameters[1];

      // TODO: lookup credentials, set session userid, greet player
    }
}

