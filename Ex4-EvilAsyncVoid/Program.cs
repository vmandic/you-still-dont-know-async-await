using System;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;

// 4. Why you should not use async void methods (and lambdas)?
namespace Ex4_EvilAsyncVoid
{
  class Program
  {
    static void Main(string[] args)
    {
      WriteLineWithThreadId("[START] Before 'VoidMethodAsync()'...");

      try
      {
        // "Some people just want to watch the world burn..."
        VoidMethodAsync(); // NOTE: can't .Wait() or .Result or anything...

        // STEP 2.
        // Task.Run(new Action(async () => throw new Exception("Error!")));

        // STEP 3.
        // Task.Run(async () => throw new Exception("Error!"));
      }
      catch (Exception ex)
      {
        WriteLineWithThreadId($"Error caught: {ex.Message}, type: {ex.GetType().FullName}");
      }

      WriteLineWithThreadId("[END] Press any key to exit...");
      ReadLine();
    }

    static async void VoidMethodAsync()
    {
      WriteLineWithThreadId("Started async void...");

      await Task.Delay(3000);

      #region what should you feel about async void methods?
      /*
        ▄█          ▄████████   ▄▄▄▄███▄▄▄▄           ▄███████▄ ███    █▄     ▄████████    ▄████████         ▄████████  ▄█    █▄   ▄█   ▄█
        ███         ███    ███ ▄██▀▀▀███▀▀▀██▄        ███    ███ ███    ███   ███    ███   ███    ███        ███    ███ ███    ███ ███  ███
        ███▌        ███    ███ ███   ███   ███        ███    ███ ███    ███   ███    ███   ███    █▀         ███    █▀  ███    ███ ███▌ ███
        ███▌        ███    ███ ███   ███   ███        ███    ███ ███    ███  ▄███▄▄▄▄██▀  ▄███▄▄▄           ▄███▄▄▄     ███    ███ ███▌ ███
        ███▌      ▀███████████ ███   ███   ███      ▀█████████▀  ███    ███ ▀▀███▀▀▀▀▀   ▀▀███▀▀▀          ▀▀███▀▀▀     ███    ███ ███▌ ███
        ███         ███    ███ ███   ███   ███        ███        ███    ███ ▀███████████   ███    █▄         ███    █▄  ███    ███ ███  ███
        ███         ███    ███ ███   ███   ███        ███        ███    ███   ███    ███   ███    ███        ███    ███ ███    ███ ███  ███▌    ▄
        █▀          ███    █▀   ▀█   ███   █▀        ▄████▀      ████████▀    ███    ███   ██████████        ██████████  ▀██████▀  █▀   █████▄▄██
                                                                              ███    ███                                                ▀
      */
      #endregion

      // STEP 1.
      // throw new Exception("Error!");

      WriteLineWithThreadId("Ended async void.");
    }

    static void WriteLineWithThreadId(string output) =>
      WriteLine($"[ThreadId: { Thread.CurrentThread.ManagedThreadId }] { output }");
  }
}
