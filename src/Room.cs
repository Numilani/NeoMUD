using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using shortid;
using shortid.Configuration;

[Table("Rooms")]
public class Room {

  [Key]
  public string Id {get; set;} = ShortId.Generate(new GenerationOptions(useSpecialCharacters: false));

  public string RoomName {get;set;} = String.Empty;
  public string DefaultDescription {get;set;} = String.Empty;

  public bool IsAccessible {get;set;} = false;
  public string NotAccessibleMessage {get;set;} = "This room is not accessible right now.";
  
  // Exits - Key: Exit Name, Value: Room Id
  public Dictionary<string, string> Exits {get;} = new();

  // Lua is stored inline in DB
  public string BeforeRoomEnterLua {get;set;} = String.Empty;
  public string AfterRoomEnterLua {get;set;} = String.Empty;

  public string TickRoomLua {get;set;} = String.Empty;

  public string BeforeRoomExitLua {get;set;} = String.Empty;
  public string AfterRoomExitLua {get;set;} = String.Empty;


  public bool AddExit(string name, string id){
    if (Exits.ContainsKey(name)){
      return false;
    }
    Exits.Add(name, id);
    return true;
  }

  public bool RemoveExit(string name){
      return Exits.Remove(name);
  }
}
