using DemiCode.Mvvm;
using MvvmWorkshop.Shared;

namespace WpfApplication1
{
    public class EditPersonViewModel : TypedViewModelBase<PersonCargo>
    {
        private int _birthYear;

        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        /// <summary>
        /// Property which notifies WPF bindings about value changes.
        /// </summary>
        public int BirthYear
        {
            get { return _birthYear; }
            set
            {
                _birthYear = value;
                RaisePropertyChanged(() => BirthYear);
            }
        }

        protected override void OnInitialize()
        {
            FirstName = Model.FirstName;
            LastName = Model.LastName;
            BirthYear = Model.BirthYear;
        }
    }
}