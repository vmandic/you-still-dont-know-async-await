using System.Threading.Tasks;
using static System.Console;

// 1. What is async / await and what does the compiler rewrite it into?
namespace Ex1_WhatIsBehindAsyncAwait
{
  class Program
  {
    static void Main(string[] args)
    {
      WriteLine("[Main START] Before 'SleepAsync().Wait()'...");

      // explicitly block the running thread with .Wait()
      SleepAsync().Wait();

      WriteLine("[Main END] Press any key to exit...");
      ReadLine();
    }

    async static Task SleepAsync()
    {
      WriteLine("[SleepAsync] Before delay 3s...");
      await Task.Delay(3000);
      WriteLine("[SleepAsync] After delay...");
    }
  }
}
