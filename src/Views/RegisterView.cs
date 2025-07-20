using Microsoft.Extensions.Logging;
using NeoMUD.src;
using NeoMUD.src.Services;
using NeoMUD.src.Services.Helpers;
using Serilog;
using SuperSocket.ProtoBase;

namespace NeoMUD.src.Views;

public class RegisterView(GameSession session, UserService userSvc, ILogger<RegisterView> logger) : IView
{
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
        await session.Print($"""
            Enter a new username:
            """);
        break;
      case "requestPassword":
        await session.Print($"""

            Enter a password:
            """);
        break;
      case "verifyPassword":
        await session.Print($"""
              Verify your password: 
              """);
        break;
      case "requestEmail":
        await session.Print($"""

                (Optional) Enter an email address, in case you forget your password. Enter "NONE" to skip.
                """);
        break;
      case "finalize":
        await session.Print($"""

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
        if (await TelnetHelpers.VerifyCommandParams(pkg, session, 0))
        {
          Username = pkg.Key;
          CurrentState = "requestPassword";
          await Display();
        }
        break;
      case "requestPassword":
        if (await TelnetHelpers.VerifyCommandParams(pkg, session, 0))
        {
          Password = pkg.Key;
          CurrentState = "verifyPassword";
          await Display();
        }
        break;
      case "verifyPassword":
        if (await TelnetHelpers.VerifyCommandParams(pkg, session, 0))
        {
          if (Password != pkg.Key)
          {
            await session.Print("Passwords do not match.");
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
        if (await TelnetHelpers.VerifyCommandParams(pkg, session, 0))
        {
          Email = pkg.Key;
          try
          {
            userSvc.CreateUser(Username, Password);
            CurrentState = "finalize";
            await Display();
          }
          catch (Exception e)
          {
            logger.LogWarning(e, "Couldn't create user");
            await session.Print("Couldn't create your new user at this time - try again later.");
            CurrentState = "requestUsername";
            await Display();
          }
        }
        break;
      case "finalize":
        if (pkg.Key.ToUpper() == "LOGIN")
        {
          var user = userSvc.AttemptSignin(Username, Password);
          if (user is null){
            await session.Print("Couldn't log you in - try again later?");
            session.CloseAsync();
          }
          session.User = user;
          session.UpdateView(typeof(CharPickView));
        }
        else if (pkg.Key.ToUpper() == "EXIT")
        {
          await session.Print($"Goodbye, {Username}!");
          await session.CloseAsync();
        }
        else
        {
          await session.Print("LOGIN or EXIT, please.");
        }
        break;
    }
  }
}
