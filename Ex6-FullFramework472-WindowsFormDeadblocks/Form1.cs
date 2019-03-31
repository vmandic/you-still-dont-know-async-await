using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ex6_FullFramework472_WindowsFormDeadblocks
{
  public partial class Form1 : Form
  {
    static Random _rand = new Random();

    public Form1()
    {
      InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      Text += " BLOCKED!";

      // Blocks the UI thread and keeps it waiting, "Sync over Async."
      // Offloads action to ThreadPool thread (Task) which can be signaled inside from another Task for completion.
      var someResult = Task.Run(CalcNumberAsync).Result;
      ShowResult(someResult);

      Text = Text.Replace(" BLOCKED!", string.Empty);
    }

    private void button2_Click(object sender, EventArgs e)
    {
      Text += " DEADBLOCKED!";

      // Blocks the UI thread forever, "Deadblock".
      // Offloads the action to a single ThreadPool thread, task can not signal completion!
      var someResult = CalcNumberAsync().Result;
      ShowResult(someResult);
    }

    private async void button3_Click(object sender, EventArgs e)
    {
      // NOTE: try removing the await
      await Task.Run(() => {
        button3.Text = $"Changed text! [{DateTime.Now}]";
      });
    }

    private async void button4_Click(object sender, EventArgs e)
    {
      // disable te UI first in the current UI SynchronizationContext
      Cursor.Current = Cursors.WaitCursor;
      foreach (Control ctrl in Controls) ctrl.Enabled = false;

      // start async operation and await it in async scope
      // the async operation Task is run on a new ThreadPool thread
      // the UI thread is released, it is NOT BLOCKED (i.e not waiting)
      var someResult = await CalcNumberAsync();

      // the control and result is given back to the UI SynchronizationContext and UI Thread
      // enable the UI (as we are back on the UI context)
      Cursor.Current = Cursors.Default;
      foreach (Control ctrl in Controls) ctrl.Enabled = true;

      ShowResult(someResult);
    }

    private async Task<double> CalcNumberAsync()
    {
      await Task.Delay(3000);
      return Math.Round(_rand.NextDouble() * 1000);
    }

    private static void ShowResult(double someResult) =>
      MessageBox.Show($"[{ DateTime.Now.ToShortTimeString() }] Result is: { someResult }");
  }
}
