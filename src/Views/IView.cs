using SuperSocket.ProtoBase;

namespace NeoMUD.src.Views;

public interface IView
{
    public abstract GameSession Session {get;set;}

    public string Display();

    public void ReceiveInput(StringPackageInfo pkg);
}
