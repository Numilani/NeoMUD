using Microsoft.Extensions.Logging;
using SuperSocket.ProtoBase;

namespace NeoMUD.src.Services;

public class PlayerCommandHandler(IGameSession session, ILogger<PlayerCommandHandler> logger, AppDbContext db)
{
    public async Task ReceiveInput(StringPackageInfo pkg)
    {
        // TODO: implement
    }


}
