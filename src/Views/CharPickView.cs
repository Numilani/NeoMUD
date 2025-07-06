using NeoMUD.src;
using NeoMUD.src.Models;
using SuperSocket.ProtoBase;

namespace NeoMUD.src.Views;

public class CharPickView(GameSession session) : IView
{
    public GameSession Session { get; set; } = session;

    public string Display()
    {
        List<Character> chars = new();


        throw new NotImplementedException();
    }

    public void ReceiveInput(StringPackageInfo pkg)
    {
        throw new NotImplementedException();
    }

    
    public void ReceiveTextInput(string msg)
    {
        throw new NotImplementedException();
    }
}
