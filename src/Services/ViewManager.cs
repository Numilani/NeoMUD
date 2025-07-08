using Microsoft.Extensions.Logging;
using NeoMUD.src;
using NeoMUD.src.Views;

namespace NeoMUD.src.Services;

public class ViewManager(UserService UserSvc, CharacterService CharSvc, ILoggerFactory _logFactory)
{
  private GameSession Session;

    public void Init(GameSession s){
    Session = s;
  }

  public IView Create(Type type){
    switch (type){
      case Type login when login == typeof(LoginView):
        return new LoginView(Session, UserSvc, _logFactory.CreateLogger<LoginView>());
      case Type register when register == typeof(RegisterView):
        return new RegisterView(Session, UserSvc, _logFactory.CreateLogger<RegisterView>());
      case Type charCreate when charCreate == typeof(CharCreateView):
        return new CharCreateView();
      case Type charPick when charPick == typeof(CharPickView):
        return new CharPickView(Session, CharSvc, _logFactory.CreateLogger<CharPickView>());
      default:
        throw new Exception("Invalid View type");
    }
  }
}
