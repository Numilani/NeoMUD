using Microsoft.Extensions.Logging;
using NeoMUD.src;
using NeoMUD.src.Models;
using NeoMUD.src.Services;
using SuperSocket.ProtoBase;

namespace NeoMUD.src.Views;

public class CharPickView(GameSession session, CharacterService charSvc, ILogger<CharPickView> logger) : IView
{
  private List<(int Index, Character Character)> Characters { get; set; }

  public async Task Display()
  {
    await session.SendTelnetStringAsync($"""
################################################################################
#####                                                                      #####
#####          CHOOSE             YOUR                CHARACTER            #####
#####                                                                      #####
#####                                                                      #####
#####     0-9 to select character          NEW to create new character     #####
################################################################################
""");
    Characters = charSvc.GetCharacters(session.UserId!).Index().ToList();

    foreach (var c in Characters)
    {
      await session.SendTelnetStringAsync($"""
          ${c.Index}) - {c.Character.CharacterName}
""");
    }
  }

  public async Task ReceiveInput(StringPackageInfo pkg)
  {
    if ( pkg.Key.ToUpper() == "NEW" ){
      // TODO: move to CharCreateView
    }
    else{
      try {
        var x = Convert.ToInt32(pkg.Key.Trim());
        if (x >= 0 && Characters.Count < x){
          session.CharId = Characters[x].Character.Id;
          // TODO: move to CurrentRoomView
        }
      }
      catch (FormatException){
        await session.SendTelnetStringAsync("Invalid selection");
      }
    }

  }
}
