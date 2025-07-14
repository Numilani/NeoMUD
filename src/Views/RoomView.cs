using Microsoft.Extensions.Logging;
using NeoMUD.src.Services;
using NeoMUD.src.Services.Helpers;
using SuperSocket.ProtoBase;

namespace NeoMUD.src.Views
{
    public class RoomView(GameSession session, RoomService roomSvc, ILogger<RoomView> logger) : IView
    {
        public async Task Display()
        {
          var room = await roomSvc.GetRoom(session.Character!.CurrentRoomId ?? "00000000");

          await session.PrintLine(room.RoomName + "\n\n");
          await session.PrintLine(room.DefaultDescription);
        }

        public Task ReceiveInput(StringPackageInfo pkg)
        {
            throw new NotImplementedException();
        }
    }
}
