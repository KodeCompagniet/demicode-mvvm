using System;

namespace DemiCode.Mvvm.Wpf
{
    ///<summary>
    /// A service for executing code on a dispatcher.
    ///</summary>
    public interface IDispatcherService
    {
        ///<summary>
        /// Execute <paramref name="action"/> on the current dispatcher.
        ///</summary>
        void Execute(Action action);
    }
}