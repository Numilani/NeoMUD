using NeoMUD.src;
using NeoMUD.src.Models;
using SuperSocket.ProtoBase;

namespace NeoMUD.src.Views;

public class CharPickView(GameSession session) : IView
{
    public GameSession Session { get; set; } = session;

    public Task Display()
    {
        throw new NotImplementedException();
    }

    public Task ReceiveInput(StringPackageInfo pkg)
    {
        throw new NotImplementedException();
    }
}
