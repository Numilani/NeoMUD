using Microsoft.Extensions.Logging;
using NeoMUD.src;
using NeoMUD.src.Models;

public class UserService
{

  private AppDbContext _context;
  private ILogger<UserService> _logger;

  public UserService(AppDbContext ctx, ILogger<UserService> logger)
  {
    _context = ctx;
    _logger = logger;
  }

  public bool CreateUser(string username, string password)
  {
    if (_context.Users.Count(x => x.Username == username) > 0){
      return false;
    }
    User newUser = new(username, password);
    _context.Users.Add(newUser);

    try{
      _context.SaveChanges();
    }
    catch (Exception e){
      _logger.LogError(e, "Failed to save new user");
      return false;
    }
    return true;
  }

  public User? AttemptSignin(string username, string password)
  {
    var checkedUser = _context.Users.FirstOrDefault(x => x.Username == username);

    if (checkedUser is null){
      return null;
    }
    
    if(!BCrypt.Net.BCrypt.Verify(password, checkedUser.PasswordHash)){
      return null;
    }

    return checkedUser;
  }

  public bool DeleteUser(string username)
  {
    var user = _context.Users.FirstOrDefault(x => x.Username == username);

    if (user is null){
      return false;
    }

    user.IsActive = false;
    _context.Users.Update(user);
    
    try{
      _context.SaveChanges();
    }
    catch (Exception e){
      _logger.LogError(e, "Failed to disable user");
      return false;
    }
    return true;

  }

}
