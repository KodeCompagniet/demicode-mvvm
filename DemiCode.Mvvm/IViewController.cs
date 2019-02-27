using System;
using System.Windows.Controls;

namespace DemiCode.Mvvm
{
    ///<summary>
    /// An interface for opening views and managing command bindings for <see cref="Commands.OpenView"/> command.
    ///</summary>
    public interface IViewController
    {
        /// <summary>
        /// Registers command bindings for a <see cref="WindowView"/>.
        /// </summary>
        /// <param name="windowView">The <see cref="WindowView"/>.</param>
        void RegisterCommandBindings(WindowView windowView);

        /// <summary>
        /// Registers command bindings for a <see cref="UserControlView"/>.
        /// </summary>
        /// <param name="userControlView">The <see cref="UserControlView"/>.</param>
        void RegisterCommandBindings(UserControlView userControlView);

        /// <summary>
        /// Registers command bindings for a <see cref="PageView"/>.
        /// </summary>
        /// <param name="pageView">The <see cref="PageView"/>.</param>
        void RegisterCommandBindings(PageView pageView);

        /// <summary>
        /// Opens a view of the specified type. If the type requires a container, one must be specified.
        /// If the <paramref name="dataContext"/> argument is specified, the view's datacontext will be set.
        /// </summary>
        /// <param name="viewType">The type of view to open.</param>
        /// <param name="dataContext">The data context for the view.</param>
        /// <param name="targetContainer">The target container for the view (user control, page).</param>
        /// <param name="openModal">If <c>true</c>, the view is opened modally. Only valid for <see cref="WindowView"/>s.</param>
        void OpenView(Type viewType, object dataContext, ContentControl targetContainer, bool openModal);

        /// <summary>
        /// Returns a value indicating whether the <see cref="OpenView(object, object, ContentControl)"/> command can execute for the given arguments.
        /// </summary>
        /// <param name="viewType">The type of view to open.</param>
        /// <param name="targetContainer">The target container for the view (user control, page).</param>
        /// <param name="openModal">If <c>true</c>, determine if the view can be opened modally (only valid for <see cref="WindowView"/>s.</param>
        /// <returns><c>true</c> if the <see cref="OpenView(object, object, ContentControl)"/> command can execute for the given arguments.</returns>
        bool CanOpenView(Type viewType, ContentControl targetContainer, bool openModal);
    }
}