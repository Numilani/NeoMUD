public class NeoMUD
{

  public static void Main()
  {
    CancellationTokenSource cts = new();

    AdminMenu menu = new();
    Thread menuHandler = new(() => menu.HandleAdminMenu(ref cts));

    Thread serverTickHandler = new(new ParameterizedThreadStart(TickServer));


    menuHandler.Start();
    serverTickHandler.Start(cts.Token);
  }


  public static void TickServer(object? tokenObj)
  {
    if (tokenObj is null) return;

    CancellationToken ct = (CancellationToken)tokenObj;

    while (!ct.IsCancellationRequested)
    {
      // do server ticking logic here
    }
  }

}

class AdminMenu
{
  public void HandleAdminMenu(ref CancellationTokenSource cts)
  {
    // logic for server management (starting, stopping, configuration, viewing logs, etc)
    // goes in here.

    Console.WriteLine("""
===============================================================================
""");
    Console.WriteLine("NeoMUD 0.0.1 Alpha Build Menu\n\n");
    Console.WriteLine("x) Stop Server and Exit");
    var key = Console.ReadKey().Key;
    if (key == ConsoleKey.X)
    {
      cts.Cancel();
      Console.WriteLine("Goodbye!");
    }
  }


}
