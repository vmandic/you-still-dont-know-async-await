using System.Threading;
using System.Threading.Tasks;
using static System.Console;

// 5. Fake asynchronicity?
// Why spawn CPU bound ThreadPool tasks?
// How do you create or start a task?
// Check out what does JsonConvert.SerializeAsync() do: https://stackoverflow.com/a/15648126/1534753
namespace Ex5_TheUnnecessaryAndCumbersomeTasks
{
  class Program
  {
    static void Main(string[] args)
    {
      var answer = 0;

      answer = CalculateAsyncBad(40, 2).GetAwaiter().GetResult();
      WriteLine("The answer to everything (bad): " + answer);
      answer = 0;

      #region STEP 1. Good
      answer = CalculateAsyncGood(40, 2).GetAwaiter().GetResult();
      WriteLine("The answer to everything (good): " + answer);
      answer = 0;
      #endregion

      #region STEP 2. Better
      answer = CalculateAsyncBetter(40, 2).GetAwaiter().GetResult();
      WriteLine("The answer to everything (better): " + answer);
      answer = 0;
      #endregion

      #region STEP 3. Best
      answer = CalculateBest(40, 2);
      WriteLine("The answer to everything (best): " + answer);
      answer = 0;
      #endregion

      #region STEP 4. Realistic
      answer = CalculateAsyncRealistic(40, 2).GetAwaiter().GetResult();
      WriteLine("The answer to everything (worked hard on this): " + answer);
      #endregion
    }

    static Task<int> CalculateAsyncBad(int a, int b)
    {
      return Task.Run(() => a + b);
    }

    static Task<int> CalculateAsyncGood(int a, int b)
    {
      return Task.FromResult(a + b);
    }

    static ValueTask<int> CalculateAsyncBetter(int a, int b)
    {
      // FREEDOM TO ALL THE MEMORY!
      return new ValueTask<int>(a + b);
    }

    static int CalculateBest(int a, int b)
    {
      // why use another thread?...
      return a + b;
    }

    static Task<int> CalculateAsyncRealistic(int a, int b)
    {
      return Task.Factory.StartNew(() =>
      {
        WriteLine("'CalculateAsyncRealistic()' thread from a thread pool: " + Thread.CurrentThread.IsThreadPoolThread);

        // for some reason (i.e. bad ex developer) this is a really CPU heavy op:
        for (int i = 0; i < 100000000; i++);
        for (int i = 0; i < 100000000; i++);
        for (int i = 0; i < 100000000; i++);
        for (int i = 0; i < 100000000; i++);
        for (int i = 0; i < 100000000; i++);
        for (int i = 0; i < 100000000; i++);

        return a + b;
      }, TaskCreationOptions.LongRunning);
    }
  }
}
