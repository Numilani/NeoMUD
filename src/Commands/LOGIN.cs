using NeoMUD.src.Services;
using SuperSocket.Command;
using SuperSocket.ProtoBase;

namespace NeoMUD.src.commands;

public class LOGIN : IAsyncCommand<GameSession, StringPackageInfo>
{
  private readonly UserService _userSvc;
  private readonly ViewManager _view;

    public LOGIN(UserService usr, ViewManager view)
    {
      _userSvc = usr;
      _view = view;
    }

    public async ValueTask ExecuteAsync(GameSession session, StringPackageInfo package, CancellationToken cancellationToken)
    {
      if (package.Parameters.Length != 2){
        await session.SendTelnetStringAsync("Syntax: LOGIN <username> <password>");
        return;
      }

      var username = package.Parameters[0];
      var password = package.Parameters[1];

      var user = _userSvc.AttemptSignin(username, password);

      if (user is null){
        await session.SendTelnetStringAsync("Username or password incorrect.");
        return;
      }

      session.UserId = user.Id;
      _view.UpdateView(session, new CharPickView(session));


    }
}

