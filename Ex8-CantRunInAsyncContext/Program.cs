using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using static System.Console;

namespace Ex8_CantRunInAsyncContext
{
  class Program
  {
    static void Main(string[] args)
    {
      WriteLine("[Main START] Initializing car factory...");

      var db = new DbCars();
      var carFactory = new CarFactory(db);

      #region STEP 2. apply after STEP 1. and comment out line #14
      // var carFactory = CarFactory.CreateCarFactory(db);
      #endregion

      WriteLine("Our car factory produces the following models:\n");
      carFactory.Cars.ForEach(WriteLine);

      WriteLine("\n[END] Press any key to exit...");
      ReadLine();
    }
  }

  class CarFactory
  {
    public List<string> Cars { get; private set; } = new List<string>();

    public CarFactory(IDbCars db)
    {
      _ = db ??
        throw new ArgumentNullException(nameof(db));

      // NOTE: sync-over-async, might deadlock, i.e we are blocking!
      Cars = db.GetCarsQueryAsync().Result.ToList();
    }

    #region STEP 1.
    private CarFactory(List<string> cars)
    {
      Cars = cars ??
        throw new ArgumentNullException(nameof(cars));
    }

    public static async Task<CarFactory> CreateCarFactory(IDbCars db)
    {
      _ = db ??
        throw new ArgumentNullException(nameof(db));

      var cars = (await db.GetCarsQueryAsync()).ToList();
      var carFactory = new CarFactory(cars);

      return carFactory;
    }
    #endregion
  }

  interface IDbCars
  {
    Task<IEnumerable<string>> GetCarsQueryAsync();
  }

  class DbCars : IDbCars
  {
    public async Task<IEnumerable<string>> GetCarsQueryAsync()
    {
      await Task.Delay(2000);
      return new List<string> { "Rimac concept_two", "Opel Vectra", "Audi A8" };
    }
  }

  // another awesome helper to help you in lazy / async init:
  // ref: https://devblogs.microsoft.com/pfxteam/asynclazyt/
  public class AsyncLazy<T> : Lazy<Task<T>>
  {
    public AsyncLazy(Func<T> valueFactory) : base(
      () => Task.Factory.StartNew(valueFactory)) { }

    public AsyncLazy(Func<Task<T>> taskFactory) : base(
      () => Task.Factory.StartNew(() => taskFactory()).Unwrap()) { }

    public TaskAwaiter<T> GetAwaiter() => Value.GetAwaiter();
  }
}
