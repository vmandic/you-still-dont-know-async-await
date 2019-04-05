using System;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;

// 3. What is sync over async and why is it bad?
// How to unwrap an aggregate exception?
namespace Ex3_SyncOverAsyncAndAggrEx
{
  internal class Program
  {
    static void Main(string[] args)
    {
      try
      {
        WriteLineWithThreadId("[Main START] Before async work...");

        SyncOverAsyncCall();

        #region STEP 2.
        // a more 'evil' approach...
        // Task.Run(() => AsyncOpWhichThrows().Wait()).Wait();
        # endregion
      }
      catch (Exception ex)
      {
        WriteLineWithThreadId($@"Exception caught!

          Error: {ex.Message}
          Type: {ex.GetType().FullName}
          Inner type: {ex?.InnerException?.GetType()?.FullName ?? "no inner ex!"}
        ");
      }

      WriteLineWithThreadId("[Main END] Press any key to exit...");
      ReadLine();
    }

    static void SyncOverAsyncCall()
    {
      WriteLineWithThreadId("[SyncOverAsyncCall] Blocking...");

      // start of a new ThreadPool Thread async action and block running thread
      Task.Run(async () => // start the lambda action in async context
      {
        WriteLineWithThreadId("[SyncOverAsyncCall Task.Run] New TP Thread. Before 3s delay...");

        await Task.Delay(3000);

        #region STEP 1.
        // throw new Exception("Err!");
        #endregion

        WriteLineWithThreadId("[SyncOverAsyncCall Task.Run] Wait done. What thread am I?");
      }).Wait(); // <-- MainThread blocked! Waiting for task to complete...

      WriteLineWithThreadId("[SyncOverAsyncCall] Block over!");

      #region STEP 3.
      // }).GetAwaiter().GetResult(); // <-- WARNING: also a blocking call!
      #endregion
    }

    static async Task AsyncOpWhichThrows()
    {
      WriteLineWithThreadId("[AsyncOpWhichThrows] Before 3s delay...");

      await Task.Delay(3000);

      WriteLineWithThreadId("[AsyncOpWhichThrows] Wait done.");
      throw new Exception("Err urgh...");
    }

    static void WriteLineWithThreadId(string output) =>
      WriteLine($"[T: { Thread.CurrentThread.ManagedThreadId }] { output }");
  }
}
