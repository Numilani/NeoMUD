using System.Text;
using SuperSocket.Server.Abstractions.Session;

public static class TelnetHelpers
{
  public async static ValueTask SendTelnetStringAsync(this IAppSession session, string str){
    await session.SendAsync(Encoding.UTF8.GetBytes(str + "\r\n"));
  }

}
