using Microsoft.Extensions.Logging;
using NeoMUD.src.Models;
using NeoMUD.src.Services;
using NeoMUD.src.Services.Helpers;
using SuperSocket.ProtoBase;

namespace NeoMUD.src.Views;

public class CharPickView(GameSession session, CharacterService charSvc, ILogger<CharPickView> logger) : IView
{
  private List<(int Index, Character Character)> Characters { get; set; }

  public async Task Display()
  {
    await session.PrintLine($"""
################################################################################
#####                                                                      #####
#####          CHOOSE             YOUR                CHARACTER            #####
#####                                                                      #####
#####                                                                      #####
#####     0-9 to select character          NEW to create new character     #####
################################################################################
""");
    var cs = await charSvc.GetCharacters(session.User!.Id);
    Characters = cs.Index().ToList();

    foreach (var c in Characters)
    {
      await session.PrintLine($"""
          {c.Index}) - {c.Character.CharacterName}
""");
    }
  }

  public async Task ReceiveInput(StringPackageInfo pkg)
  {
    if (pkg.Key.ToUpper() == "NEW")
    {
      session.UpdateView(typeof(CharCreateView));
    }
    else
    {
      try
      {
        var x = Convert.ToInt32(pkg.Key.Trim());
        if (x >= 0 && x < Characters.Count)
        {
          session.Character = Characters[x].Character;
          session.UpdateView(typeof(RoomView));
        }
      }
      catch (FormatException)
      {
        await session.PrintLine("Invalid selection");
      }
    }

  }
}
