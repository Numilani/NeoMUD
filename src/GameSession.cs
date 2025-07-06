using NeoMUD.src.Models;
using NeoMUD.src.Views;
using SuperSocket.Server;

namespace NeoMUD.src;

public class GameSession : AppSession
{

  public string? UserId { get; set; }

  public IView CurrentView { get; set; }

  public bool AwaitingInput { get; set; }

    public GameSession()
    {
      CurrentView = new LoginView(this);
    }

}
