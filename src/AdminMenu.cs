namespace NeoMUD;

public class AdminMenu
{
 
  public void PrintMainMenu(ref CancellationTokenSource cts)
  {
    Console.WriteLine("""
===============================================================================
""");
    Console.WriteLine("NeoMUD 0.0.1 Alpha Build Menu\n\n");

    Console.WriteLine("l) View Logs");
    Console.WriteLine("x) Stop Server and Exit");
    var key = Console.ReadKey(true).Key;

    switch (key){
      case ConsoleKey.L:
        break;
      case ConsoleKey.X:
        break;

    }
    if (key == ConsoleKey.X)
    {
      cts.Cancel();
      Console.WriteLine("Shutdown Complete. Goodbye!");
    }
  }



}
