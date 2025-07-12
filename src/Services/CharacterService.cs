using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NeoMUD.src.Models;

namespace NeoMUD.src.Services;

public class CharacterService(AppDbContext db, ILogger<CharacterService> logger)
{
  public async Task<List<Character>> GetCharacters(string userId)
  {
    return await db.Characters.Include(x => x.User).Where(x => x.User.Id == userId).ToListAsync();
  }

  public void CreateCharacter(Character c)
  {
    db.Characters.Add(c);

    try
    {
      db.SaveChanges();
    }
    catch (Exception e)
    {
      logger.LogError(e, "Failed to save new character");
      throw;
    }
  }

}
