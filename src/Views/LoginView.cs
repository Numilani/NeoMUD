using NeoMUD.src.Services;
using SuperSocket.ProtoBase;

namespace NeoMUD.src.Views;

public class LoginView(GameSession session, UserService userSvc, ViewManager view) : IView
{
  public GameSession Session { get; set; } = session;
  private UserService UserSvc { get; set; } = userSvc;
  private ViewManager View { get; set; } = view;

  public string Display()
  {
    return $"""
################################################################################
################################################################################
###  ######  ####          #####        ########################################
###   #####  ####  ############  ######  #######################################
###    ####  ####  ############  ######  #######################################
###  #  ###  ####  ############  ######  #######################################
###  ##  ##  ####       #######  ######  #######################################
###  ###  #  ####  ############  ######  #######################################
###  ####    ####  ############  ######  #######################################
###  ######  ####          #####        ########################################
################################################################################
################################################################################
######################################  #######  ####  ######  ####         ####
######################################   #####   ####  ######  ####  #####   ###
######################################    ###    ####  ######  ####  ######  ###
######################################  #  #  #  ####  ######  ####  ######  ###
######################################  ##   ##  ####  ######  ####  ######  ###
######################################  #######  ####  ######  ####  ######  ###
######################################  #######  #####  ####  #####  #####   ###
######################################  #######  ######      ######         ####
################################################################################
# +++ === +++ === +++ === +++ === +++ ==== +++ === +++ === +++ === +++ === +++ #
#                   LOGIN <username> <password> or REGISTER                    #
# +++ === +++ === +++ === +++ === +++ ==== +++ === +++ === +++ === +++ === +++ #
""";
  }

  public async Task LOGIN(StringPackageInfo pkg)
  {
    if (pkg.Parameters.Length != 2)
    {
      await Session.SendTelnetStringAsync("Syntax: LOGIN <username> <password>");
      return;
    }

    var username = pkg.Parameters[0];
    var password = pkg.Parameters[1];

    var user = UserSvc.AttemptSignin(username, password);

    if (user is null)
    {
      await session.SendTelnetStringAsync("Username or password incorrect.");
      return;
    }

    session.UserId = user.Id;
    View.UpdateView(session, new CharPickView(session));
  }

  public async Task REGISTER()
  {
    // TODO: create RegisterView
  }

  public async Task ReceiveInput(StringPackageInfo pkg)
  {
    switch (pkg.Key.ToUpper())
    {
      case "LOGIN":
        await LOGIN(pkg);
        break;
      case "REGISTER":
        await REGISTER();
        break;
      default:
        session.SendTelnetStringAsync("Invalid command - LOGIN or REGISTER");
        break;
    }
  }
}
