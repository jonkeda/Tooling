using System;

namespace Tooling.Threading
{
    public class DefaultThreadDispatcher : IThreadDispatcher
    {
        public bool ShouldInvoke()
        {
            return false;
        }

        public void Invoke(Action action)
        {
            action.Invoke();
        }
    }
}