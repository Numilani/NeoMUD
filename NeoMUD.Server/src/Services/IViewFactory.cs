using Microsoft.Extensions.Logging;
using NeoMUD.src;
using NeoMUD.src.Views;

namespace NeoMUD.src.Services;

public class IViewFactory(UserService UserSvc, CharacterService CharSvc, RoomService RoomSvc, ILoggerFactory _logFactory)
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
        return new CharCreateView(Session, CharSvc, _logFactory.CreateLogger<CharCreateView>());
      case Type charPick when charPick == typeof(CharPickView):
        return new CharPickView(Session, CharSvc, _logFactory.CreateLogger<CharPickView>());
      case Type room when room == typeof(RoomView):
        return new RoomView(Session, RoomSvc, _logFactory.CreateLogger<RoomView>());
      default:
        throw new Exception("Invalid View type");
    }
  }
}
