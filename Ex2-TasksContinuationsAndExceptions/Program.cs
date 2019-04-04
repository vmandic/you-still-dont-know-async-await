using System;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;

// 2. How do continuations work?
// When is a Task (work delegate) executed?
// What happens when you ignore a task (which throws)?
// How do we access the inner task that carries the value?
namespace Ex2_TasksContinuationsAndExceptions
{
  class Program
  {
    // ! async Main REQUIREMENT: <LangVersion>7.1</LangVersion> in .csproj
    async static Task Main(string[] args)
    {
      WriteLineWithThreadId("[Main START] Before 'LetsWait()'...");

      var task = LetsWaitAsync();
      var result = 0;

      #region STEP 2. how do continuations work?

      WriteLineWithThreadId("[Main] Before 'task.ContinueWith()'...");
      var nextTask = task.ContinueWith((Task<int> prevTask) => // no context switch!
      {
        WriteLineWithThreadId("[Task Continuation] Hello World from continuation!");

        #region STEP 3. errors from continuations?
        //throw new Exception("Error from continuation!");
        #endregion

        return prevTask.Result + 1;

        #region STEP 4.1. inner task, comment line before
        //return Task.Run(() => prevTask.Result + 41);
        #endregion
      });

      result = await nextTask;

      #endregion

      #region STEP 4.2.
       //var unwrappedTask = nextTask.Unwrap();
       //result = await unwrappedTask;
      #endregion

      WriteLineWithThreadId($"[Main] Result is: {result}");

      WriteLineWithThreadId("[Main END] Press any key to exit...");
      ReadLine();
    }

    async static Task<int> LetsWaitAsync()
    {
      WriteLineWithThreadId("[LetsWait] Before delay 3s...");

      await Task.Delay(3000);

      WriteLineWithThreadId("[LetsWait] After delay.");

      #region STEP 1. what happens when you ignore a task?
      // throw new Exception("Err!");
      #endregion

      return 1;
    }

    static void WriteLineWithThreadId(string output) =>
      WriteLine($"[T: { Thread.CurrentThread.ManagedThreadId }] { output }");
  }
}
