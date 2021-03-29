using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tooling.Foundation.Tasks
{
    public class TaskAbortable
    {
        public static TaskAbortable StartNew(Action action)
        {
            TaskAbortable taskAbortable = new TaskAbortable();

            taskAbortable.Start(action);

            return taskAbortable;
        }

        public bool WasAborted { get; private set; }
        private CancellationTokenSource Canceller { get; set; }
        private Task Worker { get; set; }

        public void Start(Action action)
        {
            WasAborted = false;

            // start a task with a means to do a hard abort (unsafe!)
            Canceller = new CancellationTokenSource();

            Worker = Task.Factory.StartNew(() =>
            {
                try
                {
                    using (Canceller.Token.Register(Thread.CurrentThread.Abort))
                    {
                        action();
                    }
                }
                catch (ThreadAbortException)
                {
                    WasAborted = true;
                }
            }, Canceller.Token);
        }

        public void Abort()
        {
            Canceller.Cancel();
        }
    }
}
