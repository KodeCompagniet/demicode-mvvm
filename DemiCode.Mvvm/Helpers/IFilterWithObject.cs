using System;

namespace DemiCode.Mvvm.Helpers
{
    /// <summary>
    /// This interface is meant for the <see cref="WeakPredicate{T}" /> class and can be 
    /// useful if you store multiple WeakPredicate{T} instances but don't know in advance
    /// what type T represents.
    /// </summary>
    public interface IFilterWithObject
    {
        /// <summary>
        /// Executes a predicate.
        /// </summary>
        /// <param name="parameter">A parameter passed as an object,
        /// to be casted to the appropriate type.</param>
        bool FilterWithObject(object parameter);

        /// <summary>
        /// Gets a value indicating whether the Action's owner is still alive, or if it was collected
        /// by the Garbage Collector already.
        /// </summary>
        bool IsAlive { get; }

        /// <summary>
        /// Gets the Action's owner. This object is stored as a <see cref="WeakReference" />.
        /// </summary>
        object Target { get; }

        /// <summary>
        /// Sets the reference that this instance stores to null.
        /// </summary>
        void MarkForDeletion();
    }
}