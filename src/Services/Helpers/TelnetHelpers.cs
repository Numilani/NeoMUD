using System.Text;
using NeoMUD.src;
using SuperSocket.ProtoBase;
using SuperSocket.Server.Abstractions.Session;

namespace NeoMUD.src.Services.Helpers;

public static class TelnetHelpers
{

  public static char SEPARATOR { get; set; } = '#';
  public static int LINE_LENGTH { get; set; } = 80;
  public static int INNER_MARGIN { get; set; } = 5;

  public async static ValueTask Print(this IAppSession session, string str, bool unterminated = false)
  {
    await session.SendAsync(Encoding.UTF8.GetBytes(str));
    if (!unterminated) await session.SendAsync(Encoding.UTF8.GetBytes("\r\n"));
  }

  public async static ValueTask Printf(this IAppSession session, string str, bool unprettified = false, bool unbordered = false, bool unterminated = false)
  {
    if (!unprettified)
    {
      string[] outstring;
      outstring = str.Prettify();
      if (!unbordered) outstring.AddBorder();
      foreach (var line in outstring)
      {
        await session.SendAsync(Encoding.UTF8.GetBytes(line + "\r\n"));
      }
    }
    else
    {
      if (!unbordered) str.AddBorder();
      await session.SendAsync(Encoding.UTF8.GetBytes(str));
      if (!unterminated) await session.SendAsync(Encoding.UTF8.GetBytes("\r\n"));
    }
  }

  public static string SeparatorLine(this IAppSession s)
  {
    return new string(SEPARATOR, LINE_LENGTH);
  }

  public async static ValueTask ClearScreen(this IAppSession session)
  {
    await session.SendAsync(Encoding.UTF8.GetBytes("\x1b[2J"));
  }

  public static async ValueTask PrintTopBorder(this IAppSession session)
  {
    await session.Print(session.SeparatorLine());
    await session.PrintBlankLine();
  }

  public static async ValueTask PrintBlankLine(this IAppSession session, bool bordered = true)
  {
    if (bordered) await session.Print(new string(' ', LINE_LENGTH - (2 * INNER_MARGIN)).AddBorder());
    else await session.Print(new string(' ', LINE_LENGTH - (2 * INNER_MARGIN)));
  }

  public static async ValueTask PrintBottomBorder(this IAppSession session)
  {
    await session.PrintBlankLine();
    await session.Print(session.SeparatorLine());
  }

  public async static Task<bool> VerifyCommandParams(this StringPackageInfo pkg, GameSession session, int expectedParamCount, bool exact = true, string customErrorMsg = null)
  {
    if (pkg.Parameters is null && expectedParamCount == 0) return true;
    if (exact)
    {
      if (pkg.Parameters is not null && pkg.Parameters.Length != expectedParamCount)
      {
        await session.Print(customErrorMsg ?? "Invalid Syntax - try again.");
        await session.CurrentView.Display();
        return false;
      }
    }
    if (pkg.Parameters is not null && pkg.Parameters.Length < expectedParamCount)
    {
      await session.Print(customErrorMsg ?? "Invalid Syntax - try again.");
      await session.CurrentView.Display();
      return false;
    }
    return true;
  }
}
