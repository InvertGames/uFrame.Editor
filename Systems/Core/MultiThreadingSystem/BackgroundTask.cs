using System.ComponentModel;

namespace Invert.Core
{
    public class BackgroundTask
    {
        public BackgroundWorker Worker { get; set; }

        public BackgroundTask(BackgroundWorker worker)
        {
            Worker = worker;
        }

        public void Cancel()
        {
            Worker.CancelAsync();
            Worker.Dispose();
        }

        public bool IsRunning
        {
            get { return Worker.IsBusy; }
        }

    }
}