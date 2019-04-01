using System;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;

// 2. How do continuations work?
// When is the new Tasks work executed?
// What happens when you ignore a task?
// Why does an exception get 'swallowed'? (also more in async void example)
// What result is carried out in a failing continuation?
namespace Ex2_TasksContinuationsAndExceptions
{
  class Program
  {
    // ! async Main REQUIREMENT: <LangVersion>7.1</LangVersion> in .csproj
    async static Task Main(string[] args)
    {
      WriteLineWithThreadId("[START] Before 'LetsWait()'...");

      var t = LetsWait();

      #region STEP 2.
      // WriteLineWithThreadId("Before 'ContinueWith()'...");
      // t.ContinueWith(_ => // or even ConfigureAwait(false)
      // {
      #region STEP 3.
      //   throw new Exception("Error from continuation!");
      //   WriteLineWithThreadId("Hello World from continuation!");
      #endregion
      //   return 2;
      // });
      #endregion

      #region STEP 5., comment out STEP 4.
      // WriteLineWithThreadId("Result is: " + t.Result);
      #endregion

      #region STEP 4.
      // await t;
      #endregion

      WriteLineWithThreadId("[END] Press any key to exit...");
      ReadLine();
    }

    #region STEP 5.
    // async static Task<int> LetsWait()
    #endregion
    async static Task LetsWait()
    {
      WriteLineWithThreadId("Before delay in 'LetsWait()'...");
      await Task.Delay(3000);
      WriteLineWithThreadId("After delay in 'LetsWait()'.");

      #region STEP 1. what happens when you ignore a task
      // throw new Exception("Err!");
      #endregion

      #region STEP 5.
      // return 1;
      #endregion
    }

    static void WriteLineWithThreadId(string output) =>
      WriteLine($"[ThreadId: { Thread.CurrentThread.ManagedThreadId }] { output }");
  }
}
