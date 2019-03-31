using System;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;

// 2. How do continuations work?
// When is the new Tasks work executed?
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

      // STEP 2.
      // WriteLineWithThreadId("Before 'ContinueWith()'...");
      // t.ContinueWith(_ =>
      // {
      //   // STEP 3.
      //   throw new Exception("Error from continuation!");
      //   WriteLineWithThreadId("Hello World from continuation!");

      //   return 2;
      // });

      // STEP 5., comment out STEP 4.
      // WriteLineWithThreadId("Result is: " + t.Result);

      // STEP 4.
      // await t;

      WriteLineWithThreadId("[END] Press any key to exit...");
      ReadLine();
    }

    // STEP 5.
    // async static Task<int> LetsWait()
    async static Task LetsWait()
    {
      WriteLineWithThreadId("Before delay in 'LetsWait()'...");
      await Task.Delay(3000);
      WriteLineWithThreadId("After delay in 'LetsWait()'.");

      // STEP 5.
      // return 1;
    }

    static void WriteLineWithThreadId(string output) =>
      WriteLine($"[ThreadId: { Thread.CurrentThread.ManagedThreadId }] { output }");
  }
}
