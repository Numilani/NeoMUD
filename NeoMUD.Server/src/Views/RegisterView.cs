using Microsoft.Extensions.Logging;
using NeoMUD.src.Services;
using NeoMUD.src.Services.Helpers;
using SuperSocket.ProtoBase;
using static NeoMUD.src.Services.Helpers.TelnetHelpers;

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
        await session.ClearScreen();
        await session.SendSeparatorLine(session.SEPARATOR);
        await session.SendRaw("Enter a new username: ");
        break;
      case "requestPassword":
        await session.ClearScreen();
        await session.SendSeparatorLine(session.SEPARATOR);
        await session.SendRaw($"USERNAME: {Username}", true);
        await session.SendSeparatorLine(' ');
        await session.SendRaw($"Enter a password: ");
        break;
      case "verifyPassword":
        await session.SendRaw($"Verify your password:");
        break;
      case "requestEmail":
        await session.ClearScreen();
        await session.SendRaw("(Optional) Enter an email address, in case you forget your password. Enter 'NONE' to skip.");
        break;
      case "finalize":
        await session.ClearScreen();
        await session.FormMessage("SUCCESS!", StringJustification.CENTER).Send();
        await session.FormMessage("New account created with username '{Username}' and email '{Email}'.", StringJustification.CENTER).Send();
        await session.FormMessage("Type CONTINUE to log in, or EXIT to exit.", StringJustification.CENTER).Send();
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
            await session.SendRaw("Passwords do not match.", true);
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
            await session.SendRaw("Couldn't create your new user at this time - try again later.", true);
            CurrentState = "requestUsername";
            await Display();
          }
        }
        break;
      case "finalize":
        if (pkg.Key.ToUpper() == "CONTINUE")
        {
          var user = userSvc.AttemptSignin(Username, Password);
          if (user is null){
            await session.SendRaw("Couldn't log you in - try again later?", true);
            session.CloseAsync();
          }
          session.User = user;
          session.UpdateView(typeof(CharPickView));
        }
        else if (pkg.Key.ToUpper() == "EXIT")
        {
          await session.SendRaw($"Goodbye, {Username}!");
          await session.CloseAsync();
        }
        else
        {
          await session.SendRaw("CONTINUE or EXIT, please.", true);
        }
        break;
    }
  }
}
