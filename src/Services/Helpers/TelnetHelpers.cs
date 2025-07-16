using System.Text;
using NeoMUD.src;
using SuperSocket.ProtoBase;
using SuperSocket.Server.Abstractions.Session;

namespace NeoMUD.src.Services.Helpers;

public static class TelnetHelpers
{
  public async static ValueTask Print(this IAppSession session, string str, bool unterminated = false)
  {
    if (unterminated)
    {
      await session.SendAsync(Encoding.UTF8.GetBytes(str + " "));
    }
    else
    {
      await session.SendAsync(Encoding.UTF8.GetBytes(str + "\r\n"));
    }
  }

  public async static ValueTask Print(this IAppSession s, string[] str){
    foreach (var line in str){
      await Print(s, line);
    }
  }

  public static string SeparatorLine(this IAppSession s, char separator = '#', int length = 80){
    return new string(separator, length);
  }

  public async static ValueTask ClearScreen(this IAppSession session)
  {
    await session.SendAsync(Encoding.UTF8.GetBytes("\x1b[2J"));
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
