using SuperSocket.ProtoBase;

namespace NeoMUD.src.Views;

public interface IView
{
    public Task Display();

    public Task ReceiveInput(StringPackageInfo pkg);
}
