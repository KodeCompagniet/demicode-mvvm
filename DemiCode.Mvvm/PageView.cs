using System;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using DemiCode.Mvvm.Annotations;
using DemiCode.Mvvm.Validation;
using DemiCode.Mvvm.Wpf;
using System.ComponentModel;

namespace DemiCode.Mvvm
{
    /// <summary>
    /// The <see cref="PageView"/> class is the base class for all page views.
    /// </summary>
    public class PageView : Page, IViewValidation, IView
    {
        private static readonly DependencyPropertyKey ViewModelPropertyKey = DependencyHelper<PageView>.RegisterReadOnly(view => view.ViewModel);
        private static readonly DependencyProperty ViewModelTypeProperty = DependencyHelper<PageView>.Register(view => view.ViewModelType);

        ///<summary>
        /// The viewmodel instance for this view.
        ///</summary>
        public static readonly DependencyProperty ViewModelProperty = ViewModelPropertyKey.DependencyProperty;

        /// <summary>
        /// Constructs a new <see cref="PageView"/>.
        /// </summary>
        public PageView()
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                ViewHelper.InitializeComponent(this);
                return;
            }

            // Handle the various FrameworkElement events to initialize the view
            Initialized += ViewBaseInitialized;
            DataContextChanged += ViewBaseDataContextChanged;
            Loaded += ViewBaseLoaded;
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
            InitializeViewModel();
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

            // Add view validators
            ViewValidator.AddValidators(this);
        }

        /// <summary>
        /// Event handler for the <see cref="ViewModelBase.PropertyChanged"/> event
        /// of the viewmodel. Updates the view's title from the viewmodel, if
        /// the changed property is named <c>Title</c>.
        /// </summary>
        /// <param name="sender">The viewmodel.</param>
        /// <param name="e">The associated <see cref="PropertyChangedEventArgs"/>.</param>
        private void ViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
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
            var context = IocContextHelper.GetIocContext(this);
            if (context == null)
            {
                // If we get here, it is because our control is either removed from 
                // the hierarchy OR is not added to one yet. 
                // InitializeViewModel will be called again when necessary.
                return;
            }

            // Resolve the ViewModel
            var viewModelType = ViewHelper.GetViewModelType(GetType(), ViewModelType);
            if (viewModelType == null)
            {
                // No ViewModel set
                return;
            }

            var viewModel = context.Resolve(viewModelType) as ViewModelBase;
            if (viewModel == null)
                throw new InvalidOperationException("The specified ViewModelType " + ViewModelType + " does not subclass " + typeof(ViewModelBase));

            // Initialize it
            viewModel.Initialize(DataContext);

            // Wire event handlers
            WireUpViewModel(viewModel);
        }

        private void WireUpViewModel(ViewModelBase viewModel)
        {
            // Add event handlers
            viewModel.PropertyChanged += ViewModelPropertyChanged;

            // Register command bindings for the viewmodel
            var registry = new CommandBindingRegistry(CommandBindings);
            viewModel.RegisterCommandBindings(registry);

            // Update the title
            UpdateTitleFromViewModel();

            ViewModel = viewModel;

            // Get the immediate content of the view (the UI)
            var content = Content as FrameworkElement;
            if (content == null)
                return;

            // Assign the ViewModel to the DataContext of the UI
            content.DataContext = viewModel;
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
