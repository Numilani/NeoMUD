using System.Text;
using NeoMUD.src;
using SuperSocket.ProtoBase;
using SuperSocket.Server.Abstractions.Session;

namespace NeoMUD.src.Services.Helpers;

public static class TelnetHelpers
{
    public async static ValueTask PrintLine(this IAppSession session, string str)
    {
        await session.SendAsync(Encoding.UTF8.GetBytes(str + "\r\n"));
    }

    public async static ValueTask PrintWrapped(this IAppSession session, string text, int maxLineLength, string prefix = null, string suffix = null)
    {
        if (string.IsNullOrEmpty(text))
            return;

        var words = text.Split(' ');
        var wrappedText = new List<string>();
        var currentLine = new StringBuilder();

        currentLine.Append(prefix);

        foreach (var word in words)
        {
            // if the line is as long as it can get, add line to wrappedLine
            if (currentLine.Length + word.Length + 1 + suffix.Length > maxLineLength)
            {
                currentLine.Append(suffix);
                wrappedText.Add(currentLine.ToString().TrimEnd());
                currentLine.Clear();
                currentLine.Append(prefix);
            }

            // otherwise, add another word
            currentLine.Append(word + " ");
        }

        // append any remaining words to the end of the text
        if (currentLine.Length > 0)
            wrappedText.Add(currentLine.ToString().TrimEnd() + suffix);

        foreach (var l in wrappedText)
        {
            await session.PrintLine(l);
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
