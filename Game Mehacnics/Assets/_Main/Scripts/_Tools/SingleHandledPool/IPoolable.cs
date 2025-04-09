using System;

namespace _Tools.SingleHandledPool
{
    public interface IPoolable<T>
    {
        public event Action<T> OnDisable;
        public void Disable();
        public void Enable();
    }
}