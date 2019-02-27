using DemiCode.Mvvm;
using DemiCode.Mvvm.Messaging;

namespace WpfProjectTemplate.Views
{
    public class ListenerActivatedMessage : MessageBase {}

    public class MessageListenerViewModel : TypedViewModelBase<string>
    {
        private readonly IMessenger _messenger;

        public string Data { get { return Model; } }

        public MessageListenerViewModel(IMessenger messenger)
        {
            _messenger = messenger;
        }

        protected override void OnInitialize()
        {
            _messenger.Register<object>(this, OnMessage);
            _messenger.Send(new ListenerActivatedMessage());
        }

        private void OnMessage(object msg) {}

        public override string ToString()
        {
            return GetType() + " " + Data;
        }
    }
}