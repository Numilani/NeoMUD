using Microsoft.Extensions.Logging;
using NeoMUD.src.Models;
using NeoMUD.src.Services;
using NeoMUD.src.Views;
using SuperSocket.Server;
using SuperSocket.Server.Abstractions.Session;

namespace NeoMUD.src;

public interface IGameSession : IAppSession
{
  public char SEPARATOR { get; set; }
  public int LINE_LENGTH { get; set; }
  public int INNER_MARGIN { get; set; }

  public User? User { get; set; }
  public Character? Character { get; set; }

  public IView CurrentView { get; set; }
  public bool AwaitingInput { get; set; }

  public IViewFactory ViewMgr { get; set; }
  public AdminCommandHandler AdminCommands { get; set; }
  public PlayerCommandHandler PlayerCommands { get; set; }

  public void UpdateView(Type view)
  {
    CurrentView = ViewMgr.Create(view);
    CurrentView.Display();
  }
}

public class GameSession : AppSession, IGameSession
{
  public char SEPARATOR { get; set; } = '#';
  public int LINE_LENGTH { get; set; } = 80;
  public int INNER_MARGIN { get; set; } = 5;


  public User? User { get; set; }
  public Character? Character { get; set; }

  public IView CurrentView { get; set; }
  public bool AwaitingInput { get; set; }

  public virtual IViewFactory ViewMgr { get; set; }
  public virtual AdminCommandHandler AdminCommands { get; set; }
  public virtual PlayerCommandHandler PlayerCommands { get; set; }

  public GameSession(IViewFactory viewMgr, ILoggerFactory logFactory, AppDbContext db)
  {
    ViewMgr = viewMgr;
    AdminCommands = new(this, logFactory.CreateLogger<AdminCommandHandler>(), db);
    PlayerCommands = new(this, logFactory.CreateLogger<PlayerCommandHandler>(), db);
    ViewMgr.Init(this);
    CurrentView = ViewMgr.Create(typeof(LoginView));
  }
}
