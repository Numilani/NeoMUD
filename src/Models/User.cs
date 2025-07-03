using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeoMUD.src.Models;

[Table("Users")]
public class User
{

    [Key]
    public string Id { get; set; } = DatabaseHelpers.GenerateId();

    public string Username { get; set; }

    public string PasswordHash { get; set; }

    public bool IsActive {get;set;} = true;

    public User(string username, string password)
    {
        Username = username;
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
    }

    public List<Character> Characters { get; set; } = new();

}
