namespace NeoMUD.src.Views;

public interface IView
{
    public string[] ValidCommands {get;set;}
    public abstract GameSession Session {get;set;}

    public string Display();

    public void ReceiveTextInput(string msg);
}
