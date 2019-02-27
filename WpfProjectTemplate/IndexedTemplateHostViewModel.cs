using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using DemiCode.Mvvm;
using DemiCode.Mvvm.Command;
using DemiCode.Mvvm.Messaging;
using WpfProjectTemplate.Views;

namespace WpfProjectTemplate
{
    public class IndexedTemplateHostViewModel : ViewModelBase
    {
        private readonly IMessenger _messenger;
        private int _templateIndex;

        public int TemplateIndex
        {
            get { return _templateIndex; }
            set
            {
                _templateIndex = value;
                RaisePropertyChanged(() => TemplateIndex);
                RefreshRecipientsProperties();
            }
        }

        public IList<int> Indexes { get; private set; }

        public int NumberOfRecipients
        {
            get { return Recipients.Count; }
        }

        public IList<object> Recipients
        {
            get { return _messenger.Registrations<object>().ToList(); }
        }

        public ICommand RefreshRecipients { get; private set; }

        public IndexedTemplateHostViewModel(IMessenger messenger)
        {
            _messenger = messenger;
            Indexes = new[] { 0, 1, 2, 3, 4 };

            RefreshRecipients = new RelayCommand(OnRefreshRecipients);
        }

        public override void Initialize(object model)
        {
            _messenger.Register<ListenerActivatedMessage>(this, OnListenerActivated);
        }

        private void OnListenerActivated(ListenerActivatedMessage obj)
        {
            RefreshRecipientsProperties();
        }

        private void OnRefreshRecipients()
        {
            RefreshRecipientsProperties();
        }

        private void RefreshRecipientsProperties()
        {
            RaisePropertyChanged(() => Recipients);
            RaisePropertyChanged(() => NumberOfRecipients);
        }

    }
}