using System;

namespace DemiCode.Mvvm.Helpers
{
    internal interface IWeakAction
    {
        /// <summary>
        /// Sets the reference that this instance stores to null.
        /// </summary>
        void MarkForDeletion();

        /// <summary>
        /// The action target instance.
        /// </summary>
        object Target { get; }

        /// <summary>
        /// The action delegate.
        /// </summary>
        MulticastDelegate TheAction { get; }

        /// <summary>
        /// True if the target instance is still alive.
        /// </summary>
        bool IsAlive { get; }
    }
}