using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeoMUD.src.Models;

[Table("Characters")]
public class Character
{

  [Key]
  public string Id { get; set; } = DatabaseHelpers.GenerateId();

  public required string CharacterName { get; set; }
  public string CharacterDescription {get;set;} = string.Empty;

  public string CurrentRoomId {get;set;} = "00000000";
  
  public bool Online {get;set;} = false;
  public bool OfflineIdle {get;set;} = false;

  public User User { get; set; }
}
