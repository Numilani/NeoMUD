using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeoMUD.src.Models;

[Table("Characters")]
public class Character
{

  [Key]
  public string Id { get; set; } = DatabaseHelpers.GenerateId();

  public string CharacterName { get; set; }

  public User User { get; set; }
}
