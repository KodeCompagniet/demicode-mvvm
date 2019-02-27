using System;
using System.Windows;

namespace DemiCode.Mvvm
{
    /// <summary>
    /// The default runtime implementation of the <see cref="IMessageBoxService"/> interface.
    /// </summary>
    public class MessageBoxService : IMessageBoxService
    {
        /// <summary>
        /// Displays a message box that has a message and title bar caption, and returns a result.
        /// </summary>
        /// <param name="messageBoxText">The text to display.</param>
        /// <param name="caption">The caption displayed in the title bar.</param>
        /// <returns>A <see cref="MessageBoxResult"/> which indicates which button the user pressed.</returns>
        public MessageBoxResult Show(string messageBoxText, string caption)
        {
            return MessageBox.Show(messageBoxText, caption);
        }

        /// <summary>
        /// Displays a message box that has a message and title bar caption, and returns a result.
        /// </summary>
        /// <param name="messageBoxText">The text to display.</param>
        /// <param name="caption">The caption displayed in the title bar.</param>
        /// <param name="button">A <see cref="MessageBoxButton"/> value which indicates which button, or buttons, to display.</param>
        /// <param name="icon">A <see cref="MessageBoxImage"/> value which indicates which icon to display.</param>
        /// <returns>A <see cref="MessageBoxResult"/> which indicates which button the user pressed.</returns>
        public MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon)
        {
            return MessageBox.Show(messageBoxText, caption, button, icon);
        }
    }
}
