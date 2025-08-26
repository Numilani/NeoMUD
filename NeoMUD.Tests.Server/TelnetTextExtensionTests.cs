using NeoMUD.src.Services.Helpers;
using static NeoMUD.src.Services.Helpers.TelnetHelpers;

namespace NeoMUD.Tests.Server;

public class TelnetHelperTests
{
    [SetUp]
    public void Setup()
    {
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
