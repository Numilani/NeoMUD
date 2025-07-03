using shortid;
using shortid.Configuration;

namespace NeoMUD.src;

public static class DatabaseHelpers
{
    static GenerationOptions opts = new(useSpecialCharacters: false, length: 8);

    public static string GenerateId(){
      return ShortId.Generate(opts);
    }
}
