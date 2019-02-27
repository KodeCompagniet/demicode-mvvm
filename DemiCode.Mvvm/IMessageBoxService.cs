using System.Windows;

namespace DemiCode.Mvvm
{
    /// <summary>
    /// A (mockable) interface to abstract the display of message boxes. Inject into services to enable message boxes
    /// in a unit-testable way.
    /// </summary>
    public interface IMessageBoxService
    {
        /// <summary>
        /// Displays a message box that has a message and title bar caption, and returns a result.
        /// </summary>
        /// <param name="messageBoxText">The text to display.</param>
        /// <param name="caption">The caption displayed in the title bar.</param>
        /// <returns>A <see cref="MessageBoxResult"/> which indicates which button the user pressed.</returns>
        MessageBoxResult Show(string messageBoxText, string caption);

        /// <summary>
        /// Displays a message box that has a message and title bar caption, and returns a result.
        /// </summary>
        /// <param name="messageBoxText">The text to display.</param>
        /// <param name="caption">The caption displayed in the title bar.</param>
        /// <param name="button">A <see cref="MessageBoxButton"/> value which indicates which button, or buttons, to display.</param>
        /// <param name="icon">A <see cref="MessageBoxImage"/> value which indicates which icon to display.</param>
        /// <returns>A <see cref="MessageBoxResult"/> which indicates which button the user pressed.</returns>
        MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon);
    }
}