using Microsoft.Extensions.Logging;
using NeoMUD.src.Models;
using NeoMUD.src.Services;
using NeoMUD.src.Services.Helpers;
using SuperSocket.ProtoBase;

namespace NeoMUD.src.Views;

public class CharCreateView(GameSession session, CharacterService charSvc, ILogger<CharCreateView> logger) : IView
{

  private string[] States { get; set; } = ["requestName", "requestDescription", "finalize"];
  public string CurrentState { get; set; } = "requestName";

  private string name { get; set; }
  private string description { get; set; }


  public async Task Display()
  {
    switch (CurrentState)
    {
      case "requestName":
        await session.PrintLine("Enter your character's name: ");
        break;
      case "requestDescription":
        await session.PrintLine("Enter a brief, public description of your character: ");
        break;
      case "finalize":
        await session.PrintLine($"""
################################################################################
          
            NAME: {name}

            DESCRIPTION:
""");
        await session.PrintWrapped(description, 78);
        await session.PrintLine($"""

################################################################################

CONTINUE if this looks correct, RESTART if it doesn't, or EXIT to discard.
""");
        break;
    }
  }

  public async Task ReceiveInput(StringPackageInfo pkg)
  {
    switch (CurrentState)
    {
      case "requestName":
        name = $"{pkg.Key} {pkg.Body}";
        CurrentState = "requestDescription";
        await Display();
        break;
      case "requestDescription":
        description = $"{pkg.Key} {pkg.Body}";
        CurrentState = "finalize";
        await Display();
        break;
      case "finalize":
        switch (pkg.Key.ToUpper())
        {
          case "CONTINUE":
            Character c = new() { CharacterName = name, CharacterDescription = description, User = session.User };
            charSvc.CreateCharacter(c);
            session.UpdateView(typeof(CharPickView));
            break;
          case "RESTART":
            name = string.Empty;
            description = string.Empty;
            CurrentState = "requestName";
            await Display();
            break;
          case "EXIT":
            await session.CloseAsync();
            break;
          default:
            await session.PrintLine("CONTINUE, RESTART, or EXIT please.");
            break;
        }
        break;
    }
  }
}
