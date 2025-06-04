using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using shortid;
using shortid.Configuration;

[Table("Interactables")]
public class Interactable{

  [Key]
  public string Id {get; set;} = ShortId.Generate(new GenerationOptions(useSpecialCharacters: false));
}

