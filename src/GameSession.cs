using NeoMUD.src.Models;
using SuperSocket.Server;

namespace NeoMUD.src;

public class GameSession : AppSession
{

  public string? UserId {get;set;}

  public CurrentView CurrentView {get;set;} = CurrentView.LOGIN;

  public bool UpdateView(CurrentView view){
    if (UserId is null){
      return false;
    }
    CurrentView = view;
    return true;
  }

}
