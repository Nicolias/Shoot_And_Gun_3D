using System;

namespace GameStateMashine
{
    public abstract class BaseState
    {
        public event Action OnFinished;

        public abstract void Start();
        public abstract void Stop();

        protected void FinishState()
        {
            OnFinished?.Invoke();
        }
    }
}
