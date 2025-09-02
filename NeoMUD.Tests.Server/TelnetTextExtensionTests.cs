using NeoMUD.src.Models;
using static NeoMUD.src.Services.Helpers.TelnetHelpers;
using NeoMUD.src;
using NSubstitute;

namespace NeoMUD.Tests.Server;

public class TelnetHelperTests
{
  IGameSession _subIGameSession;

  [SetUp]
  public void Setup()
  {
    _subIGameSession = Substitute.For<IGameSession>();
    _subIGameSession.SEPARATOR.Returns('#');
    _subIGameSession.LINE_LENGTH.Returns(80);
    _subIGameSession.INNER_MARGIN.Returns(5);
  }

  [Test]
  [TestCase("", ExpectedResult = new[] { "" })]
  [TestCase("test", ExpectedResult = new[] { "#    test    #" })]
  public string[] Prettify_ShouldProperlyFormatText(string str, int lineLength = -1, StringJustification justify = StringJustification.LEFT)
  {
    var msg = new TelnetMessage(_subIGameSession);
    msg.Add(str);
    return msg.TextLines.ToArray();
  }

}
