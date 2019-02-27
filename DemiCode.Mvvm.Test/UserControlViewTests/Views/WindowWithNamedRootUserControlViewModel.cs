namespace DemiCode.Mvvm.Test.UserControlViewTests.Views
{
    public class WindowWithNamedRootUserControlViewModel : ViewModelBase
    {
        private string _someData = "Hello World";
        public string SomeData
        {
            get { return _someData; }
            set { _someData = value; RaisePropertyChanged(() => SomeData);}
        }
    }
}