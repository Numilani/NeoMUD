using NeoMUD.src;
using NeoMUD.src.Views;

namespace NeoMUD.src.Services;

public class ViewManager
{

    public void UpdateView(GameSession session, IView view) {
      session.CurrentView = view;
      session.CurrentView.Display();
    }
}
