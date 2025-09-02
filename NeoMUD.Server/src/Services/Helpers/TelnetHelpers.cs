using System.Text;
using SuperSocket.ProtoBase;
using SuperSocket.Server.Abstractions.Session;
using NeoMUD.src.Models;

namespace NeoMUD.src.Services.Helpers;

public static class TelnetHelpers
{
  public enum StringJustification { LEFT, CENTER, RIGHT }

  public static Dictionary<string, string> FORMAT_MAPPING = new(){
{"<CLEAR>", "\x1b[0m"},
{"<zBLACK>", "\x1b[30m"},
{"<DARKRED>", "\x1b[31m"},
{"<DARKGREEN>", "\x1b[32m"},
{"<DARKYELLOW>", "\x1b[33m"},
{"<DARKBLUE>", "\x1b[34m"},
{"<DARKMAGENTA>", "\x1b[35m"},
{"<DARKCYAN>", "\x1b[36m"},
{"<DARKWHITE>", "\x1b[37m"},
{"<BLACK2>", "\x1b[90m"},
{"<RED>", "\x1b[91m"},
{"<GREEN>", "\x1b[92m"},
{"<YELLOW>", "\x1b[93m"},
{"<BLUE>", "\x1b[94m"},
{"<MAGENTA>", "\x1b[95m"},
{"<CYAN>", "\x1b[96m"},
{"<WHITE>", "\x1b[97m"},
{"<BOLD>", "\x1b[1m"},
{"<UNDERLINE>", "\x1b[4m"}
};

  public static TelnetMessage FormMessage(this IGameSession s)
  {
    return new TelnetMessage(s);
  }

  public static TelnetMessage FormMessage(this IGameSession s, string msg)
  {
    var x = new TelnetMessage(s);
    x.Add(msg);
    return x;
  }

  public static TelnetMessage FormMessage(this IGameSession s, string msg, StringJustification justification)
  {
    var x = new TelnetMessage(s);
    x.Add(msg, -1, justification);
    return x;
  }

  public static async ValueTask SendRaw(this IAppSession session, string str, bool terminate = false)
  {
    if (terminate) await session.SendAsync(Encoding.UTF8.GetBytes(str + "\r\n"));
    else await session.SendAsync(Encoding.UTF8.GetBytes(str));
  }

  public async static ValueTask ClearScreen(this IAppSession session)
  {
    await session.SendAsync(Encoding.UTF8.GetBytes("\x1b[2J"));
  }

  public static async ValueTask SendSeparatorLine(this IAppSession session, char separator, bool wrapped = true)
  {
    if (wrapped) await SendRaw(session, ((IGameSession)session).SEPARATOR + new string(separator, ((IGameSession)session).LINE_LENGTH - 2) + ((IGameSession)session).SEPARATOR);
  }

  public async static Task<bool> VerifyCommandParams(this StringPackageInfo pkg, IGameSession session, int expectedParamCount, bool exact = true, string customErrorMsg = null)
  {
    if (pkg.Parameters is null && expectedParamCount == 0) return true;
    if (exact)
    {
      if (pkg.Parameters is not null && pkg.Parameters.Length != expectedParamCount)
      {
        await session.SendRaw(customErrorMsg ?? "Invalid Syntax - try again.\r\n");
        await session.CurrentView.Display();
        return false;
      }
    }
    if (pkg.Parameters is not null && pkg.Parameters.Length < expectedParamCount)
    {
      await session.SendRaw(customErrorMsg ?? "Invalid Syntax - try again.\r\n");
      await session.CurrentView.Display();
      return false;
    }
    return true;
  }
}
