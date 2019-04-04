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
      WriteLineWithThreadId("[Main START] Before 'VoidMethodAsync()'...");

      try
      {
        // "Some people just want to watch the world burn..."
        VoidMethodAsync(); // NOTE: can't .Wait() or .Result or anything...

        #region STEP 2.
        // Task.Run(new Action(async () => throw new Exception("Error!")));
        #endregion

        #region STEP 3.
        // Task.Run(async () => throw new Exception("Error!"));
        #endregion
      }
      catch (Exception ex)
      {
        WriteLineWithThreadId($"[Main] Error caught: {ex.Message}, type: {ex.GetType().FullName}");
      }

      WriteLineWithThreadId("[Main END] Press any key to exit...");
      ReadLine();
    }

    static async void VoidMethodAsync()
    {
      WriteLineWithThreadId("[VoidMethodAsync] Started async void...");

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

      #region STEP 1.
      //throw new Exception("Error!");
      #endregion

      WriteLineWithThreadId("[VoidMethodAsync] Ended async void.");
    }

    static void WriteLineWithThreadId(string output) =>
      WriteLine($"[T: { Thread.CurrentThread.ManagedThreadId }] { output }");
  }
}
