using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeoMUD.src.Models;

[Table("Rooms")]
public class Room
{

    [Key]
    public string Id { get; set; } = DatabaseHelpers.GenerateId(); 

    public string RoomName { get; set; } = string.Empty;
    public string DefaultDescription { get; set; } = string.Empty;

    public bool IsAccessible { get; set; } = false;
    public string NotAccessibleMessage { get; set; } = "This room is not accessible right now.";

    // Exits - Key: Exit Name, Value: Room Id
    public Dictionary<string, string> Exits { get; } = new();

    // Lua is stored inline in DB
    public string? BeforeRoomEnterLua { get; set; }
    public string? AfterRoomEnterLua { get; set; }

    public string? TickRoomLua { get; set; }

    public string? BeforeRoomExitLua { get; set; }
    public string? AfterRoomExitLua { get; set; }


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
