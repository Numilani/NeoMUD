using Microsoft.Extensions.Logging;
using SuperSocket.ProtoBase;

namespace NeoMUD.src.Views
{
    public class RoomView(GameSession session, ILogger<RoomView> logger) : IView
    {
        public Task Display()
        {
            throw new NotImplementedException();
        }

        public Task ReceiveInput(StringPackageInfo pkg)
        {
            throw new NotImplementedException();
        }
    }
}
