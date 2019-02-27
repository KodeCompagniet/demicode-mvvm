using System.Windows.Input;

namespace DemiCode.Mvvm
{
    /// <summary>
    /// The <see cref="ViewModelCommandBinding"/> class is a <see cref="CommandBinding"/>
    /// registered for a viewmodel by the <see cref="CommandBindingRegistry"/>.
    /// </summary>
    /// <remarks>
    /// When the <see cref="ViewBase.ViewModel"/> property of <see cref="ViewBase"/> changes,
    /// any existing command bindings for the old view model must be removed from the
    /// <see cref="ViewBase.CommandBindings"/> collection. This class primarily facilitates
    /// recognizing these commands bindings so they can be removed.
    /// </remarks>
    public class ViewModelCommandBinding : CommandBinding
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Windows.Input.CommandBinding"/> class.
        /// </summary>
        public ViewModelCommandBinding()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Windows.Input.CommandBinding"/> class by
        /// using the specified <see cref="T:System.Windows.Input.ICommand"/>.
        /// </summary>
        /// <param name="command">The command to base the new <see cref="T:System.Windows.Input.RoutedCommand"/> on.</param>
        public ViewModelCommandBinding(ICommand command)
            : base(command)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Windows.Input.CommandBinding"/> class by
        /// using the specified <see cref="T:System.Windows.Input.ICommand"/> and the specified
        /// <see cref="E:System.Windows.Input.CommandBinding.Executed"/> event handler.
        /// </summary>
        /// <param name="command">The command to base the new <see cref="T:System.Windows.Input.RoutedCommand"/> on.</param>
        /// <param name="executed">The handler for the <see cref="E:System.Windows.Input.CommandBinding.Executed"/> event
        /// on the new <see cref="T:System.Windows.Input.RoutedCommand"/>.</param>
        public ViewModelCommandBinding(ICommand command, ExecutedRoutedEventHandler executed)
            : base(command, executed)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Windows.Input.CommandBinding"/> class by using the specified <see cref="T:System.Windows.Input.ICommand"/> and the specified <see cref="E:System.Windows.Input.CommandBinding.Executed"/> and <see cref="E:System.Windows.Input.CommandBinding.CanExecute"/> even handlers.
        /// </summary>
        /// <param name="command">The command to base the new <see cref="T:System.Windows.Input.RoutedCommand"/> on.</param>
        /// <param name="executed">The handler for the <see cref="E:System.Windows.Input.CommandBinding.Executed"/> event
        /// on the new <see cref="T:System.Windows.Input.RoutedCommand"/>.</param>
        /// <param name="canExecute">The handler for the <see cref="E:System.Windows.Input.CommandBinding.CanExecute"/> event
        /// on the new <see cref="T:System.Windows.Input.RoutedCommand"/>.</param>
        public ViewModelCommandBinding(ICommand command, ExecutedRoutedEventHandler executed, CanExecuteRoutedEventHandler canExecute)
            : base(command, executed, canExecute)
        {
        }
    }
}
