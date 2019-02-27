using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows;
using DemiCode.Mvvm.Annotations;
using DemiCode.Mvvm.Validation;
using DemiCode.Mvvm.Wpf;
using System.ComponentModel;
using Autofac;

namespace DemiCode.Mvvm
{
    /// <summary>
    /// The <see cref="WindowView"/> class is the base class for all window views.
    /// </summary>
    public class WindowView : Window, IView
    {
        private static readonly DependencyProperty ViewModelTypeProperty = DependencyHelper<WindowView>.Register(view => view.ViewModelType);
        private static readonly DependencyPropertyKey ViewModelPropertyKey = DependencyHelper<WindowView>.RegisterReadOnly(view => view.ViewModel);

        private ILifetimeScope _context;

        ///<summary>
        /// The viewmodel instance for this view.
        ///</summary>
        public static readonly DependencyProperty ViewModelProperty = ViewModelPropertyKey.DependencyProperty;

        /// <summary>
        /// Constructs a new <see cref="WindowView"/>.
        /// </summary>
        public WindowView() : this(null)
        {
        }

        /// <summary>
        /// Constructs a new <see cref="WindowView"/> with the specified IOC context.
        /// </summary>
        /// <param name="context">The <see cref="IComponentContext">IOC context</see></param>
        public WindowView(ILifetimeScope context)
        {
            // Handle the various FrameworkElement events to initialize the view
            Initialized += ViewBaseInitialized;
            DataContextChanged += ViewBaseDataContextChanged;
            Loaded += ViewBaseLoaded;
            Unloaded += ViewBaseUnloaded;

            // Set the IOC context
            if (context != null)
            {
                // Create our own lifetime scope for this window
                _context = context.BeginLifetimeScope("window");
                IocContextHelper.SetIocContext(this, context);
            }

            ViewHelper.InitializeComponent(this);
        }

        private void ViewBaseUnloaded(object sender, RoutedEventArgs e)
        {
            ViewHelper.UnregisterFromMessenger(ViewModel, _context);

            // Release our container lifetime scope
            var context = Interlocked.Exchange(ref _context, null);
            if (context != null)
                context.Dispose();
        }

        /// <summary>
        /// Gets or sets the <see cref="Type"/> of the <see cref="ViewModel"/> which the view
        /// will resolve and attach.
        /// </summary>
        public Type ViewModelType
        {
            get { return GetValue(ViewModelTypeProperty) as Type; }
            set { SetValue(ViewModelTypeProperty, value); }
        }

        /// <summary>
        /// Gets the viewmodel hosted in the view.
        /// </summary>
        public ViewModelBase ViewModel
        {
            get { return GetValue(ViewModelProperty) as ViewModelBase; }
            private set { SetValue(ViewModelPropertyKey, value); }
        }

        #region Event handlers

        /// <summary>
        /// Event handler for the <see cref="FrameworkElement.Initialized"/> event.
        /// </summary>
        /// <param name="sender">The view.</param>
        /// <param name="e">The associated <see cref="EventArgs"/>.</param>
        private void ViewBaseInitialized(object sender, EventArgs e)
        {
            // TODO: Inspect IocLifetimeScope property (bool)
            // If true, call IocContextHelper.SetIocContext(this) = IocContextHelper.GetIocContext(this).BeginLifetimeScope();
            // Problem: how to dispose the lifetime scope? Unloaded event seems inappropriate.
            // See http://msdn.microsoft.com/en-us/library/system.windows.frameworkelement.unloaded.aspx
        }

        /// <summary>
        /// Event handler for the <see cref="FrameworkElement.DataContextChanged"/> event.
        /// Resolves and initializes the ViewModel with the new <see cref="FrameworkElement.DataContext"/>.
        /// </summary>
        /// <param name="sender">The view.</param>
        /// <param name="e">The associated <see cref="DependencyPropertyChangedEventArgs"/>.</param>
        private void ViewBaseDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // Resolve the ViewModel and initialize it with the DataContext
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                InitializeViewModel();
            }
        }

        /// <summary>
        /// Event handler for the <see cref="ViewBaseLoaded"/> event.
        /// Resolves and initializes the ViewModel.
        /// </summary>
        /// <param name="sender">The view.</param>
        /// <param name="e">The associated <see cref="RoutedEventArgs"/>.</param>
        private void ViewBaseLoaded(object sender, RoutedEventArgs e)
        {
            // If a DataContext has been set, the ViewModel is already initialized
            if (ViewModel == null)
            {
                // Resolve and initialize the ViewModel
                InitializeViewModel();
            }

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                // Add view validators
                ViewValidator.AddValidators(this);

                // Register command bindings so the view can handle view commands (OpenView etc.)
                var context = IocContextHelper.GetIocContext(this);
                if (context != null)
                {
                    var viewController = context.Resolve<IViewController>();
                    viewController.RegisterCommandBindings(this);
                }
            }
        }

        /// <summary>
        /// Event handler for the <see cref="ViewModelBase.PropertyChanged"/> event
        /// of the viewmodel. Updates the view's title from the viewmodel, if
        /// the changed property is named <c>Title</c>.
        /// </summary>
        /// <param name="sender">The viewmodel.</param>
        /// <param name="e">The associated <see cref="PropertyChangedEventArgs"/>.</param>
        private void ViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Title")
            {
                UpdateTitleFromViewModel();
            }
        }

        #endregion

        #region Initialization and wireup

        private void InitializeViewModel()
        {
            // Get the ViewModel type
            var viewModelType = ViewHelper.GetViewModelType(GetType(), ViewModelType);
            if (viewModelType == null)
            {
                // No ViewModel type set
                return;
            }

            // Create the ViewModel
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                InitializeDesignTimeViewModel(viewModelType);
            }
            else
            {
                InitializeRuntimeViewModel(viewModelType);
            }
        }

        private void InitializeRuntimeViewModel(Type viewModelType)
        {
            // Runtime: Resolve the ViewModel using the IOC container

            // First, if we already have a viewmodel, make sure it is automatically unregistered with our current messanger instace.
            ViewHelper.UnregisterFromMessenger(ViewModel, _context);

            // Get the IOC context
            var context = IocContextHelper.GetIocContext(this);
            if (context == null)
            {
                // If we get here, it is because our control is either removed from 
                // the hierarchy OR is not added to one yet. 
                // InitializeViewModel will be called again when necessary.
                return;
            }

            var viewModel = context.Resolve(viewModelType) as ViewModelBase;
            if (viewModel == null)
                throw new InvalidOperationException("The specified ViewModelType " + ViewModelType + " does not subclass " + typeof(ViewModelBase));

            // Initialize it
            viewModel.Initialize(DataContext);

            // Wire up the view model
            WireUpViewModel(viewModel, false);
        }

        private void InitializeDesignTimeViewModel(Type viewModelType)
        {
            // Design time:
            // No IOC context is available, therefore an instance must be created from reflection. No services can be injected.
            // The ViewModel must actively support the Visual Studio designer by implementing the design-mode constructor
            // The design-mode constructor cannot take any services, and so the design-mode of a ViewModel
            // will be essentially service-less.

            var designTimeContext = new DesignTimeContext();

            ViewModelBase viewModel = null;

            // Look for a design-mode constructor
            if (viewModelType.GetConstructor(new[] { typeof(IDesignTimeContext) }) != null)
            {
                try
                {
                    viewModel = Activator.CreateInstance(viewModelType, designTimeContext) as ViewModelBase;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(String.Format(CultureInfo.InvariantCulture,
                                                  "Design-mode creation of viewmodel {1} for view {0} failed: {2} - {3}.", this.GetType().Name, viewModelType.Name, ex.GetType().Name, ex.Message));
                }
            }

            if (viewModel == null)
            {
                return;
            }

            // Initialize it
            viewModel.Initialize(designTimeContext.SampleModel);

            // Wire up the view model for design time
            WireUpViewModel(viewModel, true);
        }

        private void WireUpViewModel(ViewModelBase viewModel, bool designTime)
        {
            // Another view model instance may already be wired up.
            if (ViewModel != null)
            {
                // Remove any existing command bindings
                CommandBindingRegistry.RemoveBindingsFrom(CommandBindings);
                // Detach the PropertyChanged event handler
                ViewModel.PropertyChanged -= ViewModelPropertyChanged;
                // Detach (actually replace) the close window action
                viewModel.CloseView = () => { };
            }

            // Store the new viewmodel in the ViewModel DP
            ViewModel = viewModel;

            // Get the immediate content of the view (the UI)
            var content = Content as FrameworkElement;
            if (content != null)
            {
                // Assign the ViewModel to the DataContext of the UI
                content.DataContext = viewModel;
            }

            // Register command bindings for the viewmodel
            if (!designTime)
            {
                var registry = new CommandBindingRegistry(CommandBindings);
                viewModel.RegisterCommandBindings(registry);
            }

            // Add event handlers
            if (!designTime)
            {
                // Attach a handler for the property changed event
                viewModel.PropertyChanged += ViewModelPropertyChanged;
                // Attach an action to close the window
                viewModel.CloseView = () => this.Close();

                // Update the title
                UpdateTitleFromViewModel();
            }
        }

        private void UpdateTitleFromViewModel()
        {
            var viewModel = ViewModel;
            if (viewModel == null)
                return;

            var titleProperty = viewModel.GetType().GetProperty("Title", typeof(string));
            if (titleProperty == null)
                return;

            // The ViewModel exists and has a title - it overrides any value set in XAML
            Title = titleProperty.GetValue(viewModel, null) as string ?? String.Empty;
        }

        #endregion

        #region IViewValidation Members

        /// <summary>
        /// Return true if all view validation returns successfully.
        /// </summary>
        public bool IsValid
        {
            get { return ViewValidator.IsValid(this); }
        }

        #endregion

        #region NotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
