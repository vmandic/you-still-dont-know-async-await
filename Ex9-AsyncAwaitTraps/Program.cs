using System;
using System.Threading.Tasks;
using static System.Console;

namespace Ex9_AsyncAwaitTraps
{
  class Program
  {
    async static Task Main(string[] args)
    {
      var result = await AsyncOpBad1();
      WriteLine($"Some async nesting result: {result}");

      WriteLine("[END] The end!");
    }

    #region STEP 1. unnecessary async nesting, try-catch without await
    async static Task<int> AsyncOpBad1()
    {
      await Task.Delay(1000);
      return await AsyncOpBad2();
    }

    async static Task<int> AsyncOpBad2()
    {
      return await AsyncOpBad3();
    }

    async static Task AsyncOpBad3()
    {
      return await Task.Run(() => 42);
    }

    #region TRAP 1.sln
    async static Task<int> AsyncOpGood1()
    {
      await Task.Delay(1000);
      return AsyncOpGood2();
    }

    static Task<int> AsyncOpGood2()
    {
      return AsyncOpGood3();
    }

    async static Task AsyncOpGood3()
    {
      return Task.Run(() => 42);
    }
    #endregion
    #endregion

    #region TRAP 2. hidden danger of ForEach extension
    // TODO
    #endregion

    #region TRAP 3. sync-over-async with Streams
    // TODO
    #endregion

    #region TRAP 4. overparallelization
    // TODO
    #endregion

  }
}
