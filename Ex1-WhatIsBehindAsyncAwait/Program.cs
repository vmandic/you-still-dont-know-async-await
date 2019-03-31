using System;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;

// 1. What is async / await and what does the compiler rewrite it into?
namespace Ex1_WhatIsBehindAsyncAwait
{
  class Program
  {
    static void Main(string[] args)
    {
      WriteLine("[START] Before 'SleepAsync().Wait()'...");

      // explicitly block the running thread with .Wait()
      SleepAsync().Wait();

      WriteLine("[END] Press any key to exit...");
      Console.ReadLine();
    }

    async static Task SleepAsync()
    {
      WriteLine("Before delay...");
      await Task.Delay(3000);
      WriteLine("After delay...");
    }
  }
}
