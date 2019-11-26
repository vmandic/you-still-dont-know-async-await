using System;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Ex7_FullFramework472_AspNetMvcDeadblocks.Controllers
{
  public class HomeController : Controller
  {
    private static readonly Random rand = new Random();

    public ActionResult Index()
    {
      return View();
    }

    [Route("~/block")]
    public ActionResult BlockRunningThread()
    {
      // NOTE: sync-over-async, Task.Run runs on TaskScheduler.Default (ThreadPool)
      var someResult = Task.Run(CalcNumberAsync).Result;

      return new ContentResult
      {
        Content = $"<h1>Result (blocking) is: {someResult}</h1>",
        ContentEncoding = Encoding.UTF8,
        ContentType = "text/html"
      };
    }

    [Route("~/deadlock")]
    public ActionResult DeadlockRunningThread()
    {
      // Task runs on ASP.NET SynchronizationContext (synchronization occurrs, needs to context switch or block!)
      var someResult = CalcNumberAsync().Result;
      return null; // NOTE: will never get to here anyways...
    }

    [Route("~/async-await")]
    public async Task<ActionResult> AsyncAwait()
    {
      var someResult = await CalcNumberAsync();

      return new ContentResult
      {
        Content = $"<h1>Result (async-await) is: {someResult}</h1>",
        ContentEncoding = Encoding.UTF8,
        ContentType = "text/html"
      };
    }

    private async Task<double> CalcNumberAsync()
    {
      await Task.Delay(3000);
      return Math.Round(rand.NextDouble() * 1000);
    }
  }
}
