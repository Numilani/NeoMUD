using NeoMUD.src.Models;
using NeoMUD.src.Views;
using SuperSocket.Server;

namespace NeoMUD.src;

public class GameSession : AppSession
{

  public string? UserId { get; set; }

  public IView CurrentView { get; set; }

  public bool AwaitingInput { get; set; }

  private string _stringInput;

    public GameSession()
    {
      CurrentView = new LoginView(this);
    }

    public string StringInput
  {
    get => _stringInput;
    set
    {
      _stringInput = value;
      CurrentView.ReceiveTextInput(_stringInput);
    }
  }


}
