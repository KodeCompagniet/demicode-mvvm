using System;
using System.Windows.Input;

namespace DemiCode.Mvvm
{
    /// <summary>
    /// Interface for registering command bindings for the view model.
    /// </summary>
    public interface ICommandBindingRegistry
    {
        /// <summary>
        /// Adds a binding for the specified command to the specified implementation.
        /// </summary>
        /// <typeparam name="T">The type of parameter passed to the command implementation.</typeparam>
        /// <param name="command">The command to bind to.</param>
        /// <param name="execute">The method which executes the command.</param>
        void Add<T>(RoutedUICommand command, Action<T> execute);

        /// <summary>
        /// Adds a binding for the specified command to the specified implementation.
        /// </summary>
        /// <typeparam name="T">The type of parameter passed to the command implementation.</typeparam>
        /// <param name="command">The command to bind to.</param>
        /// <param name="execute">The method which executes the command.</param>
        /// <param name="canExecute">A method which determines whether the command is currently available.</param>
        void Add<T>(RoutedUICommand command, Action<T> execute, Predicate<T> canExecute);

        /// <summary>
        /// Adds a binding for the specified command to the specified parameterless implementation.
        /// </summary>
        /// <param name="command">The command to bind to.</param>
        /// <param name="execute">The method which executes the command.</param>
        void Add(RoutedUICommand command, Action execute);

        /// <summary>
        /// Adds a binding for the specified command to the specified parameterless implementation.
        /// </summary>
        /// <param name="command">The command to bind to.</param>
        /// <param name="execute">The method which executes the command.</param>
        /// <param name="canExecute">A method which determines whether the command is currently available.</param>
        void Add(RoutedUICommand command, Action execute, Func<bool> canExecute);
    }
}