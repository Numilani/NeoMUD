using Microsoft.Extensions.Logging;
using NeoMUD.src.Models;
using NeoMUD.src.Services;
using NeoMUD.src.Views;
using SuperSocket.Server;

namespace NeoMUD.src;

public class GameSession : AppSession
{
  public User? User { get; set; }
  public Character? Character { get; set; }

  public IView CurrentView { get; set; }
  public bool AwaitingInput { get; set; }

  public IViewFactory ViewMgr;
  public AdminCommandHandler AdminCommands;
  public PlayerCommandHandler PlayerCommands;

  public GameSession(IViewFactory viewMgr, ILoggerFactory logFactory, AppDbContext db)
  {
    ViewMgr = viewMgr;
    AdminCommands = new(this, logFactory.CreateLogger<AdminCommandHandler>(), db);
    PlayerCommands = new(this, logFactory.CreateLogger<PlayerCommandHandler>(), db);
    ViewMgr.Init(this);
    CurrentView = ViewMgr.Create(typeof(LoginView));
  }

  public void UpdateView(Type view)
  {
    CurrentView = ViewMgr.Create(view);
    CurrentView.Display();
  }

}
