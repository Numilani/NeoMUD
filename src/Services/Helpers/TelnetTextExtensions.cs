using System.Text;

namespace NeoMUD.src.Services.Helpers;

public static class TelnetTextExtensions
{

  public static string AddBorder(this string str, string prefix = "#    ", string suffix = "    #")
  {
    return prefix + str + suffix;
  }

  public static string[] AddBorder(this string[] str, string prefix = "#    ", string suffix = "    #")
  {
    for (int i = 0; i < str.Length; i++)
    {
      str[i] = prefix + str[i] + suffix;
    }
    return str;
  }

  public enum StringJustification { LEFT, CENTER, RIGHT }

  public static Dictionary<string, string> FORMAT_MAPPING = new(){
{"<CLEAR>", "\x1b[0m"},
{"<BLACK>", "\x1b[30m"},
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

  public static string ColorbracesToAnsi(this string str)
  {
    // TODO: replace colorbraces with ANSI codes
    // this is really slow, so this function should only be called
    // when saving something to DB.
    return str;
  }

  public static string AnsiToColorbraces(this string str)
  {
    // TODO: replace ANSI codes with colorbraces
    // this is ALSO really slow, so this function should only be called
    // when loading a file into the editor.
    return str;
  }

  public static string[] Prettify(this string str, int lineLength, StringJustification justify)
  {
    if (string.IsNullOrEmpty(str))
      return [str];

    var words = str.Split(' ');
    var wrappedText = new List<string>();
    var currentLine = new StringBuilder();

    foreach (var word in words)
    {
      // if the line is as long as it can get, add line to wrappedLine
      if (currentLine.Length + word.Length + 1 > lineLength)
      {
        switch (justify)
        {
          case StringJustification.LEFT:
            while (currentLine.Length < lineLength)
            {
              currentLine.Append(' ');
            }
            break;
          case StringJustification.CENTER:
            while (currentLine.Length % 2 == 0 && lineLength - currentLine.Length >= 2)
            {
              currentLine.Insert(0, ' ');
              currentLine.Append(' ');
            }
            break;
          case StringJustification.RIGHT:
            while (currentLine.Length < lineLength)
            {
              currentLine.Insert(0, ' ');
            }
            break;
          default:
            throw new Exception("cannot justify text");
        }
        wrappedText.Add(currentLine.ToString());
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
          while (currentLine.Length < lineLength)
          {
            currentLine.Append(' ');
          }
          break;
        case StringJustification.CENTER:
          while (currentLine.Length % 2 == 0 && lineLength - currentLine.Length >= 2)
          {
            currentLine.Insert(0, ' ');
            currentLine.Append(' ');
          }
          break;
        case StringJustification.RIGHT:
          while (currentLine.Length < lineLength)
          {
            currentLine.Insert(0, ' ');
          }
          break;
        default:
          throw new Exception("cannot justify text");
      }
      wrappedText.Add(currentLine.ToString());
    }


    return [.. wrappedText];
  }
}
