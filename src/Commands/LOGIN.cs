using NeoMUD.src.Models;
using SuperSocket.Command;
using SuperSocket.ProtoBase;

namespace NeoMUD.src.commands;

public class LOGIN : IAsyncCommand<GameSession, StringPackageInfo>
{
  private readonly UserService _userSvc;

    public LOGIN(UserService usr)
    {
      _userSvc = usr;
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
      session.UpdateView(CurrentView.LOGIN_CHAR_PICK);

      // TODO: lookup credentials, set session userid, greet player
    }
}

