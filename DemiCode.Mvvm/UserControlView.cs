using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using DemiCode.Mvvm.Annotations;
using DemiCode.Mvvm.Helpers;
using DemiCode.Mvvm.Messaging;
using DemiCode.Mvvm.Validation;
using DemiCode.Mvvm.Wpf;
using System.ComponentModel;
using WpfValidation = System.Windows.Controls.Validation;

namespace DemiCode.Mvvm
{
    /// <summary>
    /// The <see cref="UserControlView"/> class is the base class for all user control views.
    /// </summary>
    public class UserControlView : UserControl, IView
    {
        private static readonly DependencyProperty ViewModelTypeProperty = DependencyHelper<UserControlView>.Register(view => view.ViewModelType);
        private static readonly DependencyPropertyKey ViewModelPropertyKey = DependencyHelper<UserControlView>.RegisterReadOnly(view => view.ViewModel);
        private static readonly DependencyPropertyKey TitlePropertyKey = DependencyHelper<UserControlView>.RegisterReadOnly(view => view.Title);

        private static object _disconnectedItem;
        private IMessenger _messenger;

        static UserControlView()
        {
            CacheSentinelObject();
        }

        ///<summary>
        /// The viewmodel instance for this view.
        ///</summary>
        public static readonly DependencyProperty ViewModelProperty = ViewModelPropertyKey.DependencyProperty;

        ///<summary>
        /// The title of this view.
        ///</summary>
        public static readonly DependencyProperty TitleProperty = TitlePropertyKey.DependencyProperty;



        /// <summary>
        /// Constructs a new <see cref="UserControlView"/>.
        /// </summary>
        public UserControlView()
        {
            // Handle the various FrameworkElement events to initialize the view
            Initialized += ViewBaseInitialized;
            DataContextChanged += ViewBaseDataContextChanged;
            Loaded += ViewBaseLoaded;
            Unloaded += ViewBaseUnloaded;
            WpfValidation.AddErrorHandler(this, ErrorHandler);

            // Component initialization
            ViewHelper.InitializeComponent(this);
        }


        private bool? _errorHandlerIsValidCache;
        private void ErrorHandler(object sender, ValidationErrorEventArgs validationErrorEventArgs)
        {
            _errorHandlerIsValidCache = null;

            if (validationErrorEventArgs.Action == ValidationErrorEventAction.Added)
            {
                _errorHandlerIsValidCache = false;
            }

            try
            {
                OnPropertyChanged(NotifyPropertyChangedHelper.GetPropertyNameFromExpression(() => IsValid));
            }
            finally
            {
                _errorHandlerIsValidCache = null;
            }
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

        /// <summary>
        /// Gets title for the view. Reflects the title exposed by the ViewModel.
        /// </summary>
        public string Title
        {
            get { return (GetValue(TitleProperty) as string) ?? string.Empty; }
        }

        /// <summary>
        /// Occurs when the <see cref="Title"/> property changes for this view.
        /// </summary>
        public event EventHandler TitleChanged;

        #region Event handlers

        /// <summary>
        /// Event handler for the <see cref="FrameworkElement.Initialized"/> event.
        /// </summary>
        /// <param name="sender">The view.</param>
        /// <param name="e">The associated <see cref="EventArgs"/>.</param>
        private static void ViewBaseInitialized(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Event handler for the <see cref="FrameworkElement.DataContextChanged"/> event.
        /// Resolves and initializes the ViewModel with the new <see cref="FrameworkElement.DataContext"/>.
        /// </summary>
        /// <param name="sender">The view.</param>
        /// <param name="e">The associated <see cref="DependencyPropertyChangedEventArgs"/>.</param>
        private void ViewBaseDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsSentinelObject(DataContext))
            {
                // If the current DataContext is a sentinel object, this whole event should be ignored
                // The sentinal object is an MS.Internal.NamedObject with the value "{DisconnectedObject}"
                return;
            }

            // Resolve the ViewModel and initialize it with the DataContext
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                InitializeViewModel();
            }
        }

        /// <summary>
        /// Event handler for the <see cref="FrameworkElement.Loaded"/> event.
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

            // Add view validators (not in design)
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                ViewValidator.AddValidators(this);
            }
        }

        private void ViewBaseUnloaded(object sender, RoutedEventArgs e)
        {
            // Another view model instance may already be wired up.
            UnwireViewModel(ViewModel);
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

        /// <summary>
        /// Detects whether the specified <paramref name="dataContext"/> is a sentinel object. See:
        /// http://social.msdn.microsoft.com/Forums/en-US/wpf/thread/36aec363-9e33-45bd-81f0-1325a735cc45/#611fccf2-737f-4309-a793-4001488b23aa
        /// </summary>
        /// <param name="dataContext">The data context to test.</param>
        public bool IsSentinelObject(object dataContext)
        {
            return ReferenceEquals(_disconnectedItem, dataContext);
        }

        /// <summary>
        /// Get the (single) "disconnected item" sentinel object and caches it for later
        /// fast reference equality checks.
        /// <seealso cref="IsSentinelObject"/>
        /// </summary>
        private static void CacheSentinelObject()
        {
            // The sentinel object (or DisconnectedItem) is an instance of the internal (and so inaccesible)
            // type MS.Internal.NamedObject. Its ToString method returns "{DisconnectedItem}"
            // We cannot compare types to detect the sentinel object, because it's not accessible.
            // But we can cache a reference to the object itself, because it's a singleton

            // Turns out the sentinel object is a static field on BindingExpressionBase
            // The field is internal, so we must use reflection to get it.
            var disconnectedItemField = typeof(System.Windows.Data.BindingExpressionBase)
                .GetField("DisconnectedItem", BindingFlags.Static | BindingFlags.NonPublic);
            _disconnectedItem = disconnectedItemField != null ? disconnectedItemField.GetValue(null) : new object();

            // Throw if not found
            if (_disconnectedItem == null)
                throw new Exception("Could not get a reference to the {DisconnectedItem} sentinel object.");
        }


        /// <summary>
        /// Creates and initializes a view model of the type specified in <see cref="ViewModelType"/>.
        /// </summary>
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
            ViewModelBase viewModel = null;
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                // Design time:
                // No IOC context is available, therefore an instance must be created from reflection. No services can be injected.
                // The ViewModel must actively support the Visual Studio designer by implementing the design-mode constructor
                // The design-mode constructor cannot take any services, and so the design-mode of a ViewModel
                // will be essentially service-less (for now, anyway).
                var designTimeContext = new DesignTimeContext();

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
                            "Design-mode creation of viewmodel {1} for view {0} failed: {2} - {3}.", GetType().Name, viewModelType.Name, ex.GetType().Name, ex.Message));
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
            else
            {
                // Get the IOC context and resolve the ViewModel 
                var context = IocContextHelper.GetIocContext(this);

                if (ViewModel == null && context == null)
                {
                    // No way we can establish a new viewmodel.
                    // If we get here, it is probably because our view is either removed from 
                    // the hierarchy OR is not added to one yet. 
                    // InitializeViewModel will, in the latter case, be called again when necessary.
                    return;
                }

                // Cache a IMessenger instance in this view whenever we have a context
                if (context != null)
                {
                    CacheMessengerInstance(context);
                }

                // If we're triggered by a datacontext change AND we already have an existing ViewModel,
                // we will re-initialize the existing VM with the new datacontext
                if (ViewModel != null) // && datacontextchanged
                {
                    if (context != null)
                    {
                        UnwireViewModel();
                        viewModel = CreateViewModel(context, viewModelType);
                    }
                    else
                    {
                        viewModel = ViewModel;
                    }
                }
                else
                {
                    viewModel = CreateViewModel(context, viewModelType);
                }

                // Hook up any view->viewModel pre-requisites
                PreWireViewModel(viewModel);

                // (re-)initialize viewmodel...
                viewModel.Initialize(DataContext);

                // ...and wire up the view model to the view
                WireUpViewModel(viewModel, false);
            }
        }

        private void PreWireViewModel(ViewModelBase viewModel)
        {
            viewModel.ViewValidation = this;
        }

        private static ViewModelBase CreateViewModel(IComponentContext context, Type viewModelType)
        {
            var viewModel = context.Resolve(viewModelType) as ViewModelBase;
            if (viewModel == null)
                throw new InvalidOperationException("The specified ViewModelType " + viewModelType + " does not subclass " + typeof(ViewModelBase));
            return viewModel;
        }

        /// <summary>
        /// Resolve and save <see cref="IMessenger"/> for later use.
        /// </summary>
        private void CacheMessengerInstance(IComponentContext context)
        {
            _messenger = context.ResolveOptional<IMessenger>();
        }

        private void WireUpViewModel(ViewModelBase viewModel, bool designTime)
        {
            // Another view model instance may already be wired up.
            UnwireViewModel();

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
                viewModel.PropertyChanged += ViewModelPropertyChanged;

                // Update the title
                UpdateTitleFromViewModel();
            }
        }

        /// <summary>
        /// Unwire any existing viewmodel. 
        /// </summary>
        private void UnwireViewModel()
        {
            UnwireViewModel(ViewModel);
            ViewModel = null;
        }

        /// <summary>
        /// Unwire any existing viewmodel. 
        /// </summary>
        private void UnwireViewModel(ViewModelBase viewModel)
        {
            if (viewModel == null)
                return;

            // Remove any existing messenger registrations
            ViewHelper.UnregisterFromMessenger(viewModel, _messenger);

            // Remove any existing command bindings
            CommandBindingRegistry.RemoveBindingsFrom(CommandBindings);

            // Detach the PropertyChanged event
            viewModel.PropertyChanged -= ViewModelPropertyChanged;
        }

        private void UpdateTitleFromViewModel()
        {
            var title = String.Empty;

            var viewModel = ViewModel;
            if (viewModel != null)
            {
                var titleProperty = viewModel.GetType().GetProperty("Title", typeof(string));
                if (titleProperty != null)
                    title = titleProperty.GetValue(viewModel, null) as string ?? String.Empty;
            }

            SetValue(TitlePropertyKey, title);

            var changedHandler = TitleChanged;
            if (changedHandler != null)
                changedHandler(this, EventArgs.Empty);
        }

        #endregion

        #region Support methods for testing

        /// <summary>
        /// Used to manually trigger view reinitialization, e.g. as when datacontext changes.
        /// </summary>
        internal void ForceInitializeViewModel()
        {
            InitializeViewModel();
        }

        #endregion


        #region IViewValidation Members

        /// <summary>
        /// Return true if all view validation returns successfully.
        /// </summary>
        public bool IsValid
        {
            get
            {
                // If the current cached result is "invalid" we don't bother evaluating the view
                if (_errorHandlerIsValidCache.HasValue && !_errorHandlerIsValidCache.Value)
                    return false;

                return ViewValidator.IsValid(this);
            }
        }

        #endregion

        #region NotifyPropertyChanged

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
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
