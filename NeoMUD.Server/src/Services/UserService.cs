using Microsoft.Extensions.Logging;
using NeoMUD.src;
using NeoMUD.src.Models;

namespace NeoMUD.src.Services;

public class UserService(AppDbContext _context, ILogger<UserService> _logger)
{

    public void CreateUser(string username, string password)
    {
        if (_context.Users.Any(x => x.Username == username))
        {
            _logger.LogWarning("User attempted to register username '{username}' which already exists", username);
            throw new Exception("User already exists");
        }
        User newUser = new(username, password);
        _context.Users.Add(newUser);

        try
        {
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to save new user");
            throw;
        }
    }

    public User? AttemptSignin(string username, string password)
    {
        var checkedUser = _context.Users.FirstOrDefault(x => x.Username == username);

        if (checkedUser is null)
        {
            return null;
        }

        if (!BCrypt.Net.BCrypt.Verify(password, checkedUser.PasswordHash))
        {
            return null;
        }

        return checkedUser;
    }

    public void DeleteUser(string username)
    {
        var user = _context.Users.FirstOrDefault(x => x.Username == username);

        if (user is null)
        {
            throw new Exception("User to delete could not be found");
        }

        user.IsActive = false;
        _context.Users.Update(user);

        try
        {
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to disable user");
            throw;
        }
    }

}
