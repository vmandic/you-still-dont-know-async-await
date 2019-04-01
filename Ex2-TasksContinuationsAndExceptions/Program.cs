using System;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;

// 2. How do continuations work?
// When is the new Tasks work executed?
// What happens when you ignore a task?
// Why does an exception get 'swallowed'? (also more in async void example)
// What result is carried out in a failing continuation?
// How do we access the inner task that carries the value? await (await t)? :-)
namespace Ex2_TasksContinuationsAndExceptions
{
  class Program
  {
    // ! async Main REQUIREMENT: <LangVersion>7.1</LangVersion> in .csproj
    async static Task Main(string[] args)
    {
      WriteLineWithThreadId("[START] Before 'LetsWait()'...");

      var t = LetsWait();

      #region STEP 2. how do continuations work?
      // WriteLineWithThreadId("Before 'ContinueWith()'...");
      // t.ContinueWith(_ =>
      // {
      #region STEP 3. errors from continuations?
      //   throw new Exception("Error from continuation!");
      //   WriteLineWithThreadId("Hello World from continuation!");
      #endregion
      //   return 16;
      #region STEP 6.1. inner task, comment line before
      //    return Task.Run(() => 42);
      #endregion
      // });
      #endregion

      #region STEP 5., comment out STEP 4.
      // var result = await t;
      #endregion
      #region STEP 6.2., comment out previous STEP 5.
      // var unwrappedTask = t.Unwrap();
      // var reuslt = await unwrappedTask;
      #endregion

      // WriteLineWithThreadId($"Result is: {result}");

      #region STEP 4. resolving a task by awaiting it
      // await t;
      #endregion

      WriteLineWithThreadId("[END] Press any key to exit...");
      ReadLine();
    }

    #region STEP 5. tasks can carry values
    // async static Task<int> LetsWait()
    #endregion
    async static Task LetsWait()
    {
      WriteLineWithThreadId("Before delay in 'LetsWait()'...");
      await Task.Delay(3000);
      WriteLineWithThreadId("After delay in 'LetsWait()'.");

      #region STEP 1. what happens when you ignore a task?
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
