using System;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;

// 3. What is sync over async and why is it bad?
// How to unwrap an aggregate exception?
namespace Ex3_SyncOverAsyncAndAggrEx
{
  class Program
  {
    static void Main(string[] args)
    {
      try
      {
        WriteLineWithThreadId("[START] Before async work...");

        SomeNonAsyncLogic();

        // STEP 2.
        // Task.Run(() => AsyncOpWhichThrows().Wait()).Wait();
      }
      catch (System.Exception ex)
      {
        WriteLineWithThreadId($@"Exception caught!

          Error: {ex.Message}
          Type: {ex.GetType().FullName}
          Inner type: {ex?.InnerException?.GetType()?.FullName ?? "no inner ex!"}
        ");
      }

      WriteLineWithThreadId("[END] Press any key to exit...");
      ReadLine();
    }

    static void RunSyncOverAsync()
    {
        // start of a new ThreadPool Thread async action and block running thread
        Task.Run(async() =>
        {
          WriteLineWithThreadId("Performing async work on another ThreadPool Thread...");
          await Task.Delay(3000);

          // STEP 1.
          throw new Exception("Err!");

          WriteLineWithThreadId("Async work done!");

        }).Wait();
        // STEP 3.
        // }).GetAwaiter().GetResult();
    }

    async static Task AsyncOpWhichThrows()
    {
      await Task.Delay(3000);
      throw new Exception("Err urgh...");
    }

    static void WriteLineWithThreadId(string output) =>
      WriteLine($"[ThreadId: { Thread.CurrentThread.ManagedThreadId }] { output }");
  }
}
