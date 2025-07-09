using System.Text;
using NeoMUD.src;
using SuperSocket.ProtoBase;
using SuperSocket.Server.Abstractions.Session;

public static class TelnetHelpers
{
  public async static ValueTask PrintLine(this IAppSession session, string str)
  {
    await session.SendAsync(Encoding.UTF8.GetBytes(str + "\r\n"));
  }

public async static ValueTask PrintWrapped(this IAppSession session, string text, int maxLineLength)
{
    if (string.IsNullOrEmpty(text))
        return;

    var words = text.Split(' ');
    var wrappedLine = new List<string>();
    var line = new StringBuilder();

    foreach (var word in words)
    {
        if ((line.Length + word.Length + 1) > maxLineLength)
        {
            wrappedLine.Add(line.ToString().TrimEnd());
            line.Clear();
        }


        line.Append(word + " ");
    }

    if (line.Length > 0)
        wrappedLine.Add(line.ToString().TrimEnd());

    foreach (var l in wrappedLine){
      await PrintLine(session, l);
    }
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
        await session.PrintLine(customErrorMsg ?? "Invalid Syntax - try again.");
        await session.CurrentView.Display();
        return false;
      }
    }
    if (pkg.Parameters is not null && pkg.Parameters.Length < expectedParamCount)
    {
      await session.PrintLine(customErrorMsg ?? "Invalid Syntax - try again.");
      await session.CurrentView.Display();
      return false;
    }
    return true;
  }
}
