using System.Text;
using SuperSocket.Command;
using SuperSocket.ProtoBase;
using SuperSocket.Server.Abstractions.Session;

namespace NeoMUD.src.commands;

public class CLS : IAsyncCommand<StringPackageInfo>
{
    public async ValueTask ExecuteAsync(IAppSession session, StringPackageInfo package, CancellationToken cancellationToken)
    {
      await session.SendAsync(Encoding.UTF8.GetBytes("\x1b[2J"));
    }
}

