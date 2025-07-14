using Microsoft.Extensions.Logging;
using NeoMUD.src.Services;
using NeoMUD.src.Services.Helpers;
using SuperSocket.ProtoBase;

namespace NeoMUD.src.Views;

public class LoginView(GameSession session, UserService userSvc, ILogger<LoginView> logger) : IView
{

  public async Task Display()
  {
    await session.PrintLine($"""
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
      await session.PrintLine("Syntax: LOGIN <username> <password>");
      return;
    }

    var username = pkg.Parameters[0];
    var password = pkg.Parameters[1];

    var user = userSvc.AttemptSignin(username, password);

    if (user is null)
    {
      await session.PrintLine("Username or password incorrect.");
      return;
    }

    session.User = user;
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
        await session.PrintLine("Invalid command - LOGIN or REGISTER");
        break;
    }
  }
}
