using NeoMUD.src.Services.Helpers;
using static NeoMUD.src.Services.Helpers.TelnetTextExtensions;

namespace NeoMUD.Tests.Server;

public class TelnetTextExtensionTests
{
    [SetUp]
    public void Setup()
    {
    }

    [TestCase("", ExpectedResult = true)]
    [TestCase("hello world", ExpectedResult = true)]
    [TestCase("this line is well past the limit of eighty characters and so a border will look stupid.", ExpectedResult = true)]
    [Test]
    public bool AddBorder_ShouldAddBorder(string testStr)
    {
      var result = testStr.AddBorder();
      return result.StartsWith("#    ") && result.EndsWith("    #");
    }

    [Test]
    [TestCase("", ExpectedResult = new[]{""})]
    [TestCase("test", ExpectedResult = new[]{"#    test    #"})]
    // [TestCase()]
    public string[] Prettify_ShouldProperlyFormatText(string str, int lineLength = -1, StringJustification justify = StringJustification.LEFT)
    {
     return str.Prettify(lineLength, justify);
    }

}
