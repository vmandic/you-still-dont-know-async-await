using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;

namespace Ex9_AsyncAwaitTraps
{
  class Program
  {
    async static Task Main(string[] args)
    {
      WriteLine("[START] Starting app...");
      var result = await AsyncOpBad1();

      #region Good usage example
      // var result = await AsyncGoodOp1();
      #endregion

      WriteLine($"Some async nesting result: {result}");
      WriteLine("[END] The end!");
    }

    #region TRAP 1. unnecessary async nesting, try-catch without await
    async static Task<int> AsyncOpBad1()
    {
      await Task.Delay(1000);
      return await AsyncOpBad2();
    }

    async static Task<int> AsyncOpBad2()
    {
        return await AsyncOpBad3();
    }

    async static Task<int> AsyncOpBad3()
    {
      return await Task.Run(() => 42);
    }

    #region sln
    static Task<int> AsyncOpGood1()
    {
      return AsyncOpGood2();
    }

    static Task<int> AsyncOpGood2()
    {
      try
      {
        return AsyncOpGood3();
      }
      catch (Exception ex)
      {
        WriteLine($"Whoopsieee! {ex.Message}");
      }

      return Task.FromResult(0);
    }

    static Task<int> AsyncOpGood3()
    {
      #region TRAP 1.1.
      // throw new Exception("Err!");
      #endregion
      return Task.Run(() => 42);
    }
    #endregion
    #endregion

    #region TRAP 2. hidden danger of ForEach extension

    async static Task WriteData()
    {
      var dataPoints = new List<byte> { 0xAF, 0x4E, 0xC8, 0xF1 };

      // is this alright?
      dataPoints.ForEach(data => WriteToDiskAsync(data));

      #region is this 'alrightier'?
      //dataPoints.ForEach(WriteToDiskAsync);
      #endregion

      #region this must be it?!?!? :-)
      dataPoints.ForEach(async data => await WriteToDiskAsync(data));
      #endregion

      #region or is it this KISS/POS sln?
      // just don't do the above, also on ForEach let Eric explain it: https://blogs.msdn.microsoft.com/ericlippert/2009/05/18/foreach-vs-foreach/

      foreach (var data in dataPoints)
      {
        await WriteToDiskAsync(data);
      }
      #endregion

      async Task WriteToDiskAsync(byte data)
      {
        await Task.Delay(2000);
        WriteLine($"Successfully written to disk: '{data}'");
      }
    }
    #endregion

    #region TRAP 3. sync-over-async with Streams
    async static Task WriteStream(Stream httpContextBodyStream)
    {
      using(var streamWriter = new StreamWriter(httpContextBodyStream))
      {
        await streamWriter.WriteAsync("Hi folks! That's it for today.");

        #region open me :-) and do not forget about me
        await streamWriter.FlushAsync();
        #endregion
      }
    }
    #endregion

    // TRAP 4. Careful with someTask.ConfigureAwait(continueOnCapturedContext: false);

    // Always write ConfigureAwait(false) all the way until you return from your library code.

    // You do not need to use it anymore in ASP.NET Core.
    // ref: https://blog.stephencleary.com/2017/03/aspnetcore-synchronization-context.html


    // TRAP 5. Due to no AspNetSC in .NET Core the below code behaves
    // different in Full .NET Framework and .NET Core:
    // ref: (implicit parallelism) in previous link
    async Task<List<string>> ComputeFileNamesAsync()
    {
      var fileNames = new List<string>();

      // start all in parallel
      // (also, always favor Parallel.ForEach/For instead!)
      await Task.WhenAll(
        ComputeFileNameAsync(fileNames),
        ComputeFileNameAsync(fileNames),
        ComputeFileNameAsync(fileNames),
        ComputeFileNameAsync(fileNames),
        ComputeFileNameAsync(fileNames)
      );

      return fileNames;
    }

    async Task ComputeFileNameAsync(List<string> result)
    {
      await Task.Delay(1000);

      // continue on captured context (if exists) and block 1s
      Thread.Sleep(1000);

      result.Add(Path.GetRandomFileName());

      // on .NET Core: cca 1+1 sec to run
      // on Full .NET: cca 1+5 sec to run
    }

    // MORE TRAPS:
    // 1. https://markheath.net/post/async-antipatterns
    // 2. https://github.com/davidfowl/AspNetCoreDiagnosticScenarios/blob/master/AsyncGuidance.md
  }
}
