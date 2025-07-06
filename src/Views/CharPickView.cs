using NeoMUD.src;
using NeoMUD.src.Models;
using NeoMUD.src.Views;

public class CharPickView(GameSession session) : IView
{

    public string[] ValidCommands {get;set;} = Array.Empty<string>();
    public GameSession Session {get;set;} = session;

    public string Display()
    {
      List<Character> chars = new();


        throw new NotImplementedException();
    }

    public void ReceiveTextInput(string msg)
    {
        throw new NotImplementedException();
    }
}
