using System.Text;
using SuperSocket.Server.Abstractions.Session;
using static NeoMUD.src.Services.Helpers.TelnetHelpers;

namespace NeoMUD.src.Models;

public class TelnetMessage
{
  private IGameSession Session { get; set; }

  public List<string> TextLines { get; set; } = new();

  public TelnetMessage(IGameSession s)
  {
    Session = s;
  }

  public string ColorbracesToAnsi(string str)
  {
    // TODO: replace colorbraces with ANSI codes
    // this is really slow, so this function should only be called
    // when saving something to DB.
    return str;
  }

  public string AnsiToColorbraces(string str)
  {
    // TODO: replace ANSI codes with colorbraces
    // this is ALSO really slow, so this function should only be called
    // when loading a file into the editor.
    return str;
  }

  /// <summary> Takes in a string, adds text wrapping and borders, and adds it to the message buffer. </summary>
  public void Add(string str, int lineLength = -1, StringJustification justify = StringJustification.LEFT)
  {
    var MaxLength = lineLength != -1 ? lineLength : Session.LINE_LENGTH - (2 * Session.INNER_MARGIN);
    if (string.IsNullOrEmpty(str))
      AddSeparatorLine(' '); // is this a good idea to assume? pros: pit of success, cons: not obvious

    var words = str.Split(' ');
    var wrappedText = new List<string>();
    var currentLine = new StringBuilder();

    foreach (var word in words)
    {
      // if the line is as long as it can get, add line to wrappedLine
      if (currentLine.Length + word.Length + 1 > MaxLength)
      {
        switch (justify)
        {
          case StringJustification.LEFT:
            while (currentLine.Length < MaxLength)
            {
              currentLine.Append(' ');
            }
            break;
          case StringJustification.CENTER:
            while (currentLine.Length % 2 == 0 && MaxLength - currentLine.Length >= 2)
            {
              currentLine.Insert(0, ' ');
              currentLine.Append(' ');
            }
            break;
          case StringJustification.RIGHT:
            while (currentLine.Length < MaxLength)
            {
              currentLine.Insert(0, ' ');
            }
            break;
          default:
            throw new Exception("cannot justify text");
        }
        wrappedText.Add(
            Session.SEPARATOR + new string(' ', Session.INNER_MARGIN - 1)
            + currentLine.ToString()
            + Session.SEPARATOR + new string(' ', Session.INNER_MARGIN - 1));
        currentLine.Clear();
      }

      // otherwise, add another word
      currentLine.Append(word + " ");
    }

    // append any remaining words to the end of the text
    if (currentLine.Length > 0)
    {
      switch (justify)
      {
        case StringJustification.LEFT:
          while (currentLine.Length < MaxLength)
          {
            currentLine.Append(' ');
          }
          break;
        case StringJustification.CENTER:
          while (currentLine.Length % 2 == 0 && MaxLength - currentLine.Length >= 2)
          {
            currentLine.Insert(0, ' ');
            currentLine.Append(' ');
          }
          break;
        case StringJustification.RIGHT:
          while (currentLine.Length < MaxLength)
          {
            currentLine.Insert(0, ' ');
          }
          break;
        default:
          throw new Exception("cannot justify text");
      }
      wrappedText.Add(
                  Session.SEPARATOR + new string(' ', Session.INNER_MARGIN - 1)
                  + currentLine.ToString()
                  + Session.SEPARATOR + new string(' ', Session.INNER_MARGIN - 1));
    }

    TextLines.AddRange(wrappedText);
  }

  public void AddRaw(string str)
  {
    if (str.Length > Session.LINE_LENGTH) throw new Exception("Cannot add raw lines > 80 chars");
    else TextLines.Add(str);
  }

  public void AddSeparatorLine(char separator)
  {
    TextLines.Add(Session.SEPARATOR + new string(' ', Session.INNER_MARGIN - 1)
        + new string(separator, Session.LINE_LENGTH - (2 * Session.INNER_MARGIN))
        + Session.SEPARATOR + new string(' ', Session.INNER_MARGIN - 1));
  }

  public async ValueTask Send(StringJustification justify = StringJustification.LEFT, bool bordered = false)
  {
    if (bordered)
    {
      TextLines.Insert(0, new string(Session.SEPARATOR, Session.LINE_LENGTH));
    }
    foreach (var line in TextLines)
    {
      if (bordered) await ((IAppSession)Session).SendAsync(
          Encoding.UTF8.GetBytes(
            Session.SEPARATOR + new string(' ', Session.INNER_MARGIN - 1)
            + line
            + new string(' ', Session.INNER_MARGIN - 1) + Session.SEPARATOR
            + "\r\n"));
      else await ((IAppSession)Session).SendAsync(Encoding.UTF8.GetBytes(line + "\r\n"));
    }
    if (bordered)
    {
      TextLines.Insert(0, new string(Session.SEPARATOR, Session.LINE_LENGTH));
    }
  }

}

