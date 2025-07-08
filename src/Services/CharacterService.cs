using Microsoft.EntityFrameworkCore;
using NeoMUD.src.Models;

namespace NeoMUD.src.Services;

public class CharacterService(AppDbContext db)
{
  public List<Character> GetCharacters(string userId){
    return db.Characters.Include(x => x.User).Where(x => x.User.Id == userId).ToList();
  }

}
