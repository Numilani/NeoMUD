using SuperSocket.ProtoBase;

namespace NeoMUD.src.Views;

public interface IView
{
    public abstract GameSession Session {get;set;}

    public Task Display();

    public Task ReceiveInput(StringPackageInfo pkg);
}
