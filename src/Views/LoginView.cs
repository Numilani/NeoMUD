using Microsoft.Extensions.Logging;
using NeoMUD.src.Services;
using SuperSocket.ProtoBase;

namespace NeoMUD.src.Views;

public class LoginView(GameSession session, UserService userSvc, ILogger<LoginView> logger) : IView
{

  public async Task Display()
  {
    await session.SendTelnetStringAsync($"""
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
""");
  }

  public async Task LOGIN(StringPackageInfo pkg)
  {
    if (pkg.Parameters.Length != 2)
    {
      await session.SendTelnetStringAsync("Syntax: LOGIN <username> <password>");
      return;
    }

    var username = pkg.Parameters[0];
    var password = pkg.Parameters[1];

    var user = userSvc.AttemptSignin(username, password);

    if (user is null)
    {
      await session.SendTelnetStringAsync("Username or password incorrect.");
      return;
    }

    session.UserId = user.Id;
    session.UpdateView(typeof(CharPickView));
  }

  public void REGISTER()
  {
    session.UpdateView(typeof(RegisterView));
  }

  public async Task ReceiveInput(StringPackageInfo pkg)
  {
    switch (pkg.Key.ToUpper())
    {
      case "LOGIN":
        await LOGIN(pkg);
        break;
      case "REGISTER":
        REGISTER();
        break;
      default:
        await session.SendTelnetStringAsync("Invalid command - LOGIN or REGISTER");
        break;
    }
  }
}
