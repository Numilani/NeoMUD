using Microsoft.Extensions.Logging;
using NeoMUD.src.Services;
using NeoMUD.src.Services.Helpers;
using SuperSocket.ProtoBase;

namespace NeoMUD.src.Views
{
    public class RoomView(IGameSession session, RoomService roomSvc, ILogger<RoomView> logger) : IView
    {
        public async Task Display()
        {
          var room = await roomSvc.GetRoom(session.Character!.CurrentRoomId ?? "00000000");

          await session.SendRaw(room.RoomName + "\n\n");
          await session.SendRaw(room.DefaultDescription);
        }

        public Task ReceiveInput(StringPackageInfo pkg)
        {
            throw new NotImplementedException();
        }
    }
}
