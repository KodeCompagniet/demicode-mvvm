using System.Globalization;
using DemiCode.Mvvm;
using DemiCode.Mvvm.Messaging;

namespace WpfProjectTemplate.Views
{
    public class UntypedMessageListenerViewModel : ViewModelBase
    {
        private static int _counter;
        private readonly IMessenger _messenger;
        private object _model;

        public string Data { get; private set; }

        public UntypedMessageListenerViewModel(IMessenger messenger)
        {
            _messenger = messenger;
            _counter++;
            Data = _counter.ToString(CultureInfo.InvariantCulture);
        }

        public override void Initialize(object model)
        {
            _model = model;
            _messenger.Register<object>(this, OnMessage);
            _messenger.Send(new ListenerActivatedMessage());
        }

        private void OnMessage(object msg) {}

        public override string ToString()
        {
            return GetType() + " #" + Data + (_model != null ? " (" + _model + ")" : "");
        }
    }
}