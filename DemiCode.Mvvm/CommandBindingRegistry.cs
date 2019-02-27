using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DemiCode.Mvvm
{
    /// <summary>
    /// The <see cref="CommandBindingRegistry"/> class is used by viewmodels to
    /// add command bindings to the view. Passed by to the
    /// <see cref="ViewModelBase.RegisterCommandBindings"/> method.
    /// </summary>
    public class CommandBindingRegistry : ICommandBindingRegistry
    {
        private readonly CommandBindingCollection _commandBindingCollection;

        /// <summary>
        /// Constructs a new <see cref="CommandBindingRegistry"/>.
        /// </summary>
        /// <param name="commandBindingCollection">The <see cref="CommandBindingCollection"/>
        /// to add bindings to.</param>
        public CommandBindingRegistry(CommandBindingCollection commandBindingCollection)
        {
            _commandBindingCollection = commandBindingCollection;
        }

        /// <summary>
        /// Adds a binding for the specified <see cref="RoutedUICommand"/> to a parameterless
        /// handler, with no <c>CanExecute</c> delegate (the command will always be available).
        /// </summary>
        /// <param name="command">The <see cref="RoutedUICommand"/> to bind.</param>
        /// <param name="execute">The <see cref="Action"/> to execute when the command is invoked.</param>
        public void Add(RoutedUICommand command, Action execute)
        {
            _commandBindingCollection.Add(new ViewModelCommandBinding(command,
                                                                      (sender, e) => execute(),
                                                                      (sender, e) => e.CanExecute = true));
        }

        /// <summary>
        /// Adds a binding for the specified <see cref="RoutedUICommand"/> to a parameterless
        /// handler.
        /// </summary>
        /// <param name="command">The <see cref="RoutedUICommand"/> to bind.</param>
        /// <param name="execute">The <see cref="Action"/> to execute when the command is invoked.</param>
        /// <param name="canExecute">A <see cref="Func{Boolean}"/> which determines when the command is available.</param>
        public void Add(RoutedUICommand command, Action execute, Func<bool> canExecute)
        {
            _commandBindingCollection.Add(new ViewModelCommandBinding(command,
                                                                      (sender, e) => execute(),
                                                                      (sender, e) => e.CanExecute = canExecute()));
        }

        /// <summary>
        /// Adds a binding for the specified <see cref="RoutedUICommand"/> to a parametrized
        /// handler, with no <c>CanExecute</c> delegate (the command will always be available).
        /// </summary>
        /// <typeparam name="T">The type of parameter for the handler.</typeparam>
        /// <param name="command">The <see cref="RoutedUICommand"/> to bind.</param>
        /// <param name="execute">The <see cref="Action{T}"/> to execute when the command is invoked.</param>
        public void Add<T>(RoutedUICommand command, Action<T> execute)
        {
            _commandBindingCollection.Add(new ViewModelCommandBinding(command,
                                                                      (sender, e) => execute((T)e.Parameter),
                                                                      (sender, e) => e.CanExecute = true));
        }

        /// <summary>
        /// Adds a binding for the specified <see cref="RoutedUICommand"/> to a parametrized
        /// handler.
        /// </summary>
        /// <typeparam name="T">The type of parameter for the handler.</typeparam>
        /// <param name="command">The <see cref="RoutedUICommand"/> to bind.</param>
        /// <param name="execute">The <see cref="Action{T}"/> to execute when the command is invoked.</param>
        /// <param name="canExecute">A <see cref="Predicate{T}"/> which determines when the command is available.</param>
        public void Add<T>(RoutedUICommand command, Action<T> execute, Predicate<T> canExecute)
        {
            _commandBindingCollection.Add(new ViewModelCommandBinding(command,
                                                                      (sender, e) => execute((T)e.Parameter),
                                                                      (sender, e) => e.CanExecute = canExecute((T)e.Parameter)));
        }

        /// <summary>
        /// Removes all bindings registered by <see cref="CommandBindingRegistry"/> (all <see cref="ViewModelCommandBinding"/>s)
        /// from the specified <see cref="CommandBindingCollection"/>.
        /// </summary>
        /// <param name="commandBindingCollection">The <see cref="CommandBindingCollection"/> to clean up.</param>
        public static void RemoveBindingsFrom(CommandBindingCollection commandBindingCollection)
        {
            var bindingsToRemove = new List<ViewModelCommandBinding>();

            // Find all command bindings of type ViewModelCommandBinding
            foreach (var bindingObject in commandBindingCollection)
            {
                var binding = bindingObject as ViewModelCommandBinding;
                if (binding != null)
                    bindingsToRemove.Add(binding);
            }

            // Remove them
            foreach (var binding in bindingsToRemove)
                commandBindingCollection.Remove(binding);
        }
    }
}