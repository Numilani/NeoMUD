using Microsoft.Extensions.Logging;
using NeoMUD.src;
using Serilog;
using SuperSocket.ProtoBase;

namespace NeoMUD.src.Views;

public class RegisterView(GameSession session, UserService userSvc) : IView
{
  private UserService _userSvc = userSvc;
  public GameSession Session { get; set; } = session;

  private string[] States { get; set; } = ["requestUsername", "requestPassword", "verifyPassword", "requestEmail", "finalize"];
  public string CurrentState { get; set; } = "requestUsername";

  public string Username { get; set; }
  public string Password { get; set; }
  public string Email { get; set; } = "<NONE>";


  public async Task Display()
  {
    switch (CurrentState)
    {
      case "requestUsername":
        await Session.SendTelnetStringAsync($"""
            Enter a new username:
            """);
        break;
      case "requestPassword":
        await Session.SendTelnetStringAsync($"""

            Enter a password:
            """);
        break;
      case "verifyPassword":
        await Session.SendTelnetStringAsync($"""
              Verify your password: 
              """);
        break;
      case "requestEmail":
        await Session.SendTelnetStringAsync($"""

                (Optional) Enter an email address, in case you forget your password. Enter "NONE" to skip.
                """);
        break;
      case "finalize":
        await Session.SendTelnetStringAsync($"""

                SUCCESS!
                New account created with username '{Username}' and email '{Email}'. Type CONTINUE to log in, or EXIT to exit.
                """);
        break;
    }
  }

  public async Task ReceiveInput(StringPackageInfo pkg)
  {
    switch (CurrentState)
    {
      case "requestUsername":
        if (await TelnetHelpers.VerifyCommandParams(pkg, Session, 0))
        {
          Username = pkg.Key;
          CurrentState = "requestPassword";
          await Display();
        }
        break;
      case "requestPassword":
        if (await TelnetHelpers.VerifyCommandParams(pkg, Session, 0))
        {
          Password = pkg.Key;
          CurrentState = "verifyPassword";
          await Display();
        }
        break;
      case "verifyPassword":
        if (await TelnetHelpers.VerifyCommandParams(pkg, Session, 0))
        {
          if (Password != pkg.Key)
          {
            await Session.SendTelnetStringAsync("Passwords do not match.");
            CurrentState = "requestUsername";
            await Display();
          }
          else
          {
            CurrentState = "requestEmail";
            await Display();
          }
        }
        break;
      case "requestEmail":
        if (await TelnetHelpers.VerifyCommandParams(pkg, Session, 0))
        {
          Email = pkg.Key;
          try
          {
            _userSvc.CreateUser(Username, Password);
            CurrentState = "finalize";
            await Display();
          }
          catch (Exception e)
          {
            Log.Warning(e, "Couldn't create user");
            await Session.SendTelnetStringAsync("Couldn't create your new user at this time - try again later.");
            CurrentState = "requestUsername";
            await Display();
          }
        }
        break;
      case "finalize":
        if (pkg.Key.ToUpper() == "LOGIN")
        {
          var user = _userSvc.AttemptSignin(Username, Password);
          if (user is null){
            await Session.SendTelnetStringAsync("Couldn't log you in - try again later?");
            Session.CloseAsync();
          }
          Session.UserId = user.Id;
          await Session.SendTelnetStringAsync("Success! Login works! (DEBUG)");
          Session.CloseAsync();
          // TODO: send user to CurrentRoomView
        }
        else if (pkg.Key.ToUpper() == "EXIT")
        {
          await Session.SendTelnetStringAsync($"Goodbye, {Username}!");
          await Session.CloseAsync();
        }
        else
        {
          Session.SendTelnetStringAsync("LOGIN or EXIT, please.");
        }
        break;
    }
  }
}
