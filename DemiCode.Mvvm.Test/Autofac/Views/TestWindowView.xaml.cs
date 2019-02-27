using Autofac;

namespace DemiCode.Mvvm.Test.Autofac.Views
{
    public partial class TestWindowView 
    {
        public TestWindowView() : this(null)
        {
        }

        public TestWindowView(ILifetimeScope context) : base(context)
        {
            InitializeComponent();
        }
    }
}
