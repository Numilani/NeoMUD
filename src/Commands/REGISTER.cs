using SuperSocket.Command;
using SuperSocket.ProtoBase;

namespace NeoMUD.src.commands;

public class REGISTER : IAsyncCommand<GameSession, StringPackageInfo>
{
    public async ValueTask ExecuteAsync(GameSession session, StringPackageInfo package, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
