using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using shortid;
using shortid.Configuration;

namespace NeoMUD.src;

[Table("Rooms")]
public class Room
{

    [Key]
    public string Id { get; set; } = ShortId.Generate(new GenerationOptions(useSpecialCharacters: false));

    public string RoomName { get; set; } = string.Empty;
    public string DefaultDescription { get; set; } = string.Empty;

    public bool IsAccessible { get; set; } = false;
    public string NotAccessibleMessage { get; set; } = "This room is not accessible right now.";

    // Exits - Key: Exit Name, Value: Room Id
    public Dictionary<string, string> Exits { get; } = new();

    // Lua is stored inline in DB
    public string BeforeRoomEnterLua { get; set; } = string.Empty;
    public string AfterRoomEnterLua { get; set; } = string.Empty;

    public string TickRoomLua { get; set; } = string.Empty;

    public string BeforeRoomExitLua { get; set; } = string.Empty;
    public string AfterRoomExitLua { get; set; } = string.Empty;


    public bool AddExit(string name, string id)
    {
        if (Exits.ContainsKey(name))
        {
            return false;
        }
        Exits.Add(name, id);
        return true;
    }

    public bool RemoveExit(string name)
    {
        return Exits.Remove(name);
    }
}
