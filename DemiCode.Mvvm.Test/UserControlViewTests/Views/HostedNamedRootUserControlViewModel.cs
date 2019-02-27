using System.Collections.Generic;

namespace DemiCode.Mvvm.Test.UserControlViewTests.Views
{
    public class HostedNamedRootUserControlViewModel : ViewModelBase
    {
        public static readonly List<HostedNamedRootUserControlViewModel> Instances = new List<HostedNamedRootUserControlViewModel>();
        private string _theModel;

        public string TheModel
        {
            get { return _theModel; }
            set { _theModel = value; RaisePropertyChanged(() => TheModel);}
        }

        public HostedNamedRootUserControlViewModel() : this(null)
        {
        }

        public HostedNamedRootUserControlViewModel(IDesignTimeContext context) : base(context)
        {
            Instances.Add(this);
        }

        public override void Initialize(object model)
        {
            base.Initialize(model);

            TheModel = model as string;
        }
    }
}