using NeoMUD.src;
using NeoMUD.src.Views;

namespace NeoMUD.src.Services;

public class ViewManager(UserService UserSvc)
{
  private GameSession Session;

    public void Init(GameSession s){
    Session = s;
  }

  public IView Create(Type type){
    switch (type){
      case Type login when login == typeof(LoginView):
        return new LoginView(Session, UserSvc);
      case Type register when register == typeof(RegisterView):
        return new RegisterView(Session, UserSvc);
      default:
        throw new Exception("Invalid View type");
    }
  }
}
