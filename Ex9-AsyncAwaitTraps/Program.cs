using System;
using System.Collections.Generic;
using System.IO;
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
      dataPoints.ForEach(data => WriteToDisk(data));

      #region is this alrightier?
      // dataPoints.ForEach(WriteToDisk);
      #endregion

      #region this must be it? :-)
      // dataPoints.ForEach(async data => await WriteToDisk(data));
      #endregion

      #region or is it this POSS
      // ... just don't do the above, let Eric explain it: https://blogs.msdn.microsoft.com/ericlippert/2009/05/18/foreach-vs-foreach/
      foreach (var data in dataPoints) await WriteToDisk(data);
      #endregion

      async Task WriteToDisk(byte data)
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
  }
}
