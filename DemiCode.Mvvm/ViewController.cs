using System;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using Autofac;

namespace DemiCode.Mvvm
{
    ///<summary>
    /// Handle view command bindings.
    ///</summary>
    public class ViewController : IViewController
    {
        private readonly Func<ILifetimeScope> _lifetimeScopeFactory;

        /// <summary>
        /// Constructs a new ViewController
        /// </summary>
        /// <param name="iocContext">The context from where to resolve types</param>
        /// <param name="lifetimeScopeFactory">A factory for creating lifetime scopes</param>
        public ViewController(IComponentContext iocContext, Func<ILifetimeScope> lifetimeScopeFactory)
        {
            if (iocContext == null)
                throw new ArgumentNullException("iocContext");

            if (lifetimeScopeFactory == null)
                throw new ArgumentNullException("lifetimeScopeFactory");

            _lifetimeScopeFactory = lifetimeScopeFactory;

            IocContext = iocContext;
        }

        /// <summary>
        /// Gets the IOC context for this <see cref="ViewController"/>.
        /// </summary>
        public IComponentContext IocContext { get; private set; }

        /// <summary>
        /// Registers command bindings for a <see cref="WindowView"/>.
        /// </summary>
        /// <param name="windowView">The <see cref="WindowView"/>.</param>
        public void RegisterCommandBindings(WindowView windowView)
        {
            RegisterCommandBindings((UIElement)windowView);
        }

        /// <summary>
        /// Registers command bindings for a <see cref="UserControlView"/>.
        /// </summary>
        /// <param name="userControlView">The <see cref="UserControlView"/>.</param>
        public void RegisterCommandBindings(UserControlView userControlView)
        {
            RegisterCommandBindings((UIElement)userControlView);
        }

        /// <summary>
        /// Registers command bindings for a <see cref="PageView"/>.
        /// </summary>
        /// <param name="pageView">The <see cref="PageView"/>.</param>
        public void RegisterCommandBindings(PageView pageView)
        {
            RegisterCommandBindings((UIElement)pageView);
        }

        /// <summary>
        /// Registers command bindings for a <see cref="UIElement"/>.
        /// </summary>
        /// <param name="element">The <see cref="UIElement"/>.</param>
        private void RegisterCommandBindings(UIElement element)
        {
            element.CommandBindings.Add(new CommandBinding(Commands.OpenView, OpenViewHandler, CanOpenViewHandler));
            element.CommandBindings.Add(new CommandBinding(Commands.OpenModalView, OpenModalViewHandler, CanOpenModalViewHandler));
        }

        ///// <summary>
        ///// Opens a <see cref="WindowView"/> of the specified type.
        ///// If the <paramref name="dataContext"/> argument is specified, the view's datacontext will be set.
        ///// </summary>
        ///// <typeparam name="TView">The type of view to open.</typeparam>
        ///// <param name="dataContext">The data context for the view.</param>
        //public void OpenView<TView>(object dataContext) where TView : WindowView
        //{
        //    OpenView(typeof(TView), dataContext, null);
        //}

        ///// <summary>
        ///// Opens a view of the specified type. If the type requires a container, one must be specified.
        ///// If the <paramref name="dataContext"/> argument is specified, the view's datacontext will be set.
        ///// </summary>
        ///// <typeparam name="TView">The type of view to open.</typeparam>
        ///// <param name="dataContext">The data context for the view.</param>
        ///// <param name="targetContainer">The target container for the view (user control, page).</param>
        //public void OpenView<TView>(object dataContext, ContentControl targetContainer) where TView : UIElement, IView
        //{
        //    OpenView(typeof(TView), dataContext, targetContainer);
        //}

        ///// <summary>
        ///// Opens a view of the specified type. If the type requires a container, one must be specified.
        ///// If the <paramref name="dataContext"/> argument is specified, the view's datacontext will be set.
        ///// </summary>
        ///// <param name="viewType">The type of view to open.</param>
        ///// <param name="dataContext">The data context for the view.</param>
        ///// <param name="targetContainer">The target container for the view (user control, page).</param>
        //public void OpenView(Type viewType, object dataContext, ContentControl targetContainer)
        //{
        //    OpenView(viewType, dataContext, targetContainer, false);
        //}

        ///// <summary>
        ///// Opens a <see cref="WindowView"/> modally, ie waits for it to be closed before continuing.
        ///// If the <paramref name="dataContext"/> argument is specified, the view's datacontext will be set.
        ///// </summary>
        ///// <typeparam name="TView">The type of view to open.</typeparam>
        ///// <param name="dataContext">The data context for the view.</param>
        //public void OpenModalView<TView>(object dataContext) where TView : WindowView
        //{
        //    OpenView(typeof(TView), dataContext, null, true);
        //}

        ///// <summary>
        ///// Opens a view of the specified type modally, ie waits for it to be closed before continuing.
        ///// If the <paramref name="dataContext"/> argument is specified, the view's datacontext will be set.
        ///// This method is only valid for views that derive from <see cref="WindowView"/>.
        ///// </summary>
        ///// <param name="viewType">The type of view to open.</param>
        ///// <param name="dataContext">The data context for the view.</param>
        //public void OpenModalView(Type viewType, object dataContext)
        //{
        //    OpenView(viewType, dataContext, null, true);
        //}

        /// <summary>
        /// Opens a view of the specified type. If the type requires a container, one must be specified.
        /// If the <paramref name="dataContext"/> argument is specified, the view's datacontext will be set.
        /// </summary>
        /// <param name="viewType">The type of view to open.</param>
        /// <param name="dataContext">The data context for the view.</param>
        /// <param name="targetContainer">The target container for the view (user control, page).</param>
        /// <param name="openModal">If <c>true</c>, the view is opened modally. Only valid for <see cref="WindowView"/>s.</param>
        public void OpenView(Type viewType, object dataContext, ContentControl targetContainer, bool openModal)
        {
            if (viewType == null)
                throw new ArgumentNullException("viewType");

            object view = null;
            if (IocContext.IsRegistered(viewType))
            {
                // The view is a registered type, resolve it
                view = IocContext.Resolve(viewType);
            }
            else
            {
                // Not a registered type, just instantiate it. This requires a default constructor.
                if (viewType.GetConstructor(new Type[0]) == null)
                    throw new ArgumentException(String.Format(CultureInfo.InvariantCulture,
                        "The view type {0} cannot be instantiated because is not registered with IOC, and has no default constructor.", viewType.Name));
                view = Activator.CreateInstance(viewType);
            }

            OpenView(view, dataContext, targetContainer, openModal);
        }

        /// <summary>
        /// Returns a value indicating whether the <see cref="OpenView(object, object, ContentControl)"/> command can execute for the given arguments.
        /// </summary>
        /// <param name="viewType">The type of view to open.</param>
        /// <param name="targetContainer">The target container for the view (user control, page).</param>
        /// <param name="openModal">If <c>true</c>, determine if the view can be opened modally (only valid for <see cref="WindowView"/>s.</param>
        /// <returns><c>true</c> if the <see cref="OpenView(object, object, ContentControl)"/> command can execute for the given arguments.</returns>
        public bool CanOpenView(Type viewType, ContentControl targetContainer, bool openModal)
        {
            if (viewType == null)
                return false;

            // For WindowViews, no more is required
            if (IsWindowView(viewType))
                return true;

            // Non-WindowViews: cannot open modally
            if (openModal)
                return false;

            // For UserControlViews, a TargetContainer is required
            if (viewType.IsSubclassOf(typeof(UserControlView)))
                return (targetContainer != null);

            // For PageViews, a TargetContainer is required and must be a Frame
            if (viewType.IsSubclassOf(typeof(PageView)))
                return (targetContainer != null && targetContainer is Frame);

            // All other types are invalid
            return false;
        }

        #region OpenView Privates

        /// <summary>
        /// Opens a specified view instance as either a window view, a user control view or a page view,
        /// depending on the type of the instance.  If the type requires a container (user control, page),
        /// one must be specified.
        /// If the <paramref name="dataContext"/> argument is specified, the view's datacontext will be set.
        /// </summary>
        /// <param name="viewInstance">The view to open.</param>
        /// <param name="dataContext">The data context for the view.</param>
        /// <param name="targetContainer">The target container for the view (user control, page).</param>
        /// <param name="openModal">If <c>true</c>, the view is opened modally. Ignored for non-<see cref="WindowView"/>s.</param>
        private void OpenView(object viewInstance, object dataContext, ContentControl targetContainer, bool openModal)
        {
            var viewType = viewInstance.GetType();

            // WindowView
            if (IsWindowView(viewType))
            {
                OpenWindowView((WindowView)viewInstance, dataContext, openModal);
                return;
            }

            // UserControlView
            if (viewType.IsSubclassOf(typeof(UserControlView)))
            {
                OpenUserControlView((UserControlView)viewInstance, dataContext, targetContainer);
                return;
            }

            // PageView
            if (viewType.IsSubclassOf(typeof(PageView)))
            {
                OpenPageView((PageView)viewInstance, dataContext, targetContainer);
                return;
            }

            throw new ArgumentException("The view does not subclass any known view type (WindowView, PageView or UserControlView).", "viewInstance");
        }

        /// <summary>
        /// Opens a specified window view instance. The instance must be assignable to <see cref="WindowView"/>.
        /// If the <paramref name="dataContext"/> argument is specified, the view's datacontext will be set.
        /// </summary>
        /// <param name="view">The view to open.</param>
        /// <param name="dataContext">The data context for the view.</param>
        /// <param name="openModal">If <c>true</c>, the view is opened modally.</param>
        private void OpenWindowView(WindowView view, object dataContext, bool openModal)
        {
            // Set the view's IOC Context, if necessary
            if (IocContextHelper.GetIocContext(view) == null)
                IocContextHelper.SetIocContext(view, IocContext);

            // Set its DataContext
            view.DataContext = dataContext;

            if (openModal)
            {
                view.ShowDialog();
            }
            else
            {
                view.Show();
            }
        }

        /// <summary>
        /// Opens a specified user control view instance. The instance must be assignable to <see cref="UserControlView"/>.
        /// If the <paramref name="dataContext"/> argument is specified, the view's datacontext will be set.
        /// </summary>
        /// <param name="view">The view to open.</param>
        /// <param name="dataContext">The data context for the view.</param>
        /// <param name="targetContainer">The target container for the view.</param>
        private void OpenUserControlView(UserControlView view, object dataContext, ContentControl targetContainer)
        {
            if (targetContainer == null)
                throw new ArgumentException("Missing target Container.");

            // If the target container has no IOC context, let the new view inherit ours
            if (IocContextHelper.GetIocContext(targetContainer) == null)
                IocContextHelper.SetIocContext(view, IocContext);

            // Set the view's DataContext
            view.DataContext = dataContext;

            // Add it to the container
            targetContainer.Content = view;
        }

        /// <summary>
        /// Opens a specified page view instance. The instance must be assignable to <see cref="PageView"/>.
        /// If the <paramref name="dataContext"/> argument is specified, the view's datacontext will be set.
        /// </summary>
        /// <param name="view">The view to open.</param>
        /// <param name="dataContext">The data context for the view.</param>
        /// <param name="targetContainer">The target container for the view.</param>
        private void OpenPageView(PageView view, object dataContext, ContentControl targetContainer)
        {
            if (targetContainer == null)
                throw new ArgumentException("Missing target Container.");
            if (!(targetContainer is Frame))
                throw new ArgumentException("The target Container must be a Frame.");

            // If the target container has no IOC context, let the new view inherit ours
            if (IocContextHelper.GetIocContext(targetContainer) == null)
                IocContextHelper.SetIocContext(view, IocContext);

            // Set the view's DataContext
            view.DataContext = dataContext;

            // Add it to the container
            targetContainer.Content = view;
        }

        private static bool IsWindowView(Type viewType)
        {
            return viewType.IsSubclassOf(typeof(WindowView));
        }

        #endregion

        #region Event handlers

        private void OpenViewHandler(object sender, ExecutedRoutedEventArgs e)
        {
            var sourceElement = (UIElement)e.OriginalSource;
            // Get the view type from the source element's attached mvvm:WindowView.ViewType property
            var viewType = Commands.GetViewType(sourceElement);
            // Get the target container (if any) from the source element's attached mvvm:WindowView.TargetContainer property
            var targetContainer = Commands.GetTargetContainer(sourceElement);
            // Get the data context for the view from the command argument
            var dataContext = e.Parameter;

            OpenView(viewType, dataContext, targetContainer, true);
        }

        private void CanOpenViewHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            var sourceElement = (UIElement)e.OriginalSource;
            // Get the view type from the source element's attached mvvm:WindowView.ViewType property
            var viewType = Commands.GetViewType(sourceElement);
            // Get the target container (if any) from the source element's attached mvvm:WindowView.TargetContainer property
            var targetContainer = Commands.GetTargetContainer(sourceElement);

            e.CanExecute = CanOpenView(viewType, targetContainer, false);
        }

        private void OpenModalViewHandler(object sender, ExecutedRoutedEventArgs e)
        {
            var sourceElement = (UIElement)e.OriginalSource;
            // Get the view type from the source element's attached mvvm:WindowView.ViewType property
            var viewType = Commands.GetViewType(sourceElement);
            // Get the data context for the view from the command argument
            var dataContext = e.Parameter;

            OpenView(viewType, dataContext, null, true);
        }

        private void CanOpenModalViewHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            var sourceElement = (UIElement)e.OriginalSource;
            // Get the view type from the source element's attached mvvm:WindowView.ViewType property
            var viewType = Commands.GetViewType(sourceElement);

            e.CanExecute = CanOpenView(viewType, null, true);
        }

        #endregion
    }
}
