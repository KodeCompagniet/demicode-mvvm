using System.Collections.ObjectModel;
using DemiCode.Mvvm;
using DemiCode.Mvvm.Command;

namespace WpfProjectTemplate
{
    /// <summary>
    /// The ViewModel for the MainView.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private string _message;

        /// <summary>
        /// Constructs a new <see cref="MainViewModel"/>.
        /// TODO: Add constructor parameters for any service the ViewModel requires. The parameters will be injected by the IOC Framework.
        /// </summary>
        public MainViewModel()
        {
            HideView = new RelayCommand(OnHideView, CanHideView);
            Messages = new ObservableCollection<string>();
            Messages.CollectionChanged += (s, a) => HideView.RaiseCanExecuteChanged();
        }

        public ObservableCollection<string> Messages { get; private set; }

        /// <summary>
        /// Initializes the ViewModel with the specified model.
        /// </summary>
        /// <param name="model">The model (data).</param>
        public override void Initialize(object model)
        {
            // Use the Initialize method to fetch the data to expose to the view.
            // The model parameter will contain the DataContext of the view.
            // For top-level WindowViews like this, and for many other views, it will be null.
            // In those cases, you will likely want to fetch the data from a service.
            _message = "Greetings from the MainViewModel: Hello World!";

            Messages.Add("Hello");
        }

        private bool CanHideView()
        {
            return Messages.Count > 0;
        }

        private void OnHideView()
        {
            Messages.Clear();
        }

        /// <summary>
        /// A command that hides a view.
        /// </summary>
        public RelayCommand HideView { get; private set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                RaisePropertyChanged(() => Message);
            }
        }
    }
}
