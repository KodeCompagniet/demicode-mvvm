using DemiCode.Mvvm;

namespace WpfProjectTemplate.Views
{
    public class SomeUserControlViewModel : TypedViewModelBase<string>
    {
        private string _theModel;

        public string TheModel
        {
            get { return _theModel; }
            set { _theModel = value; RaisePropertyChanged(() => TheModel);}
        }

        protected override void OnInitialize()
        {
            TheModel = Model;
        }
    }
}