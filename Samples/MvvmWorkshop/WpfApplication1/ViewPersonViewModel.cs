using System;
using DemiCode.Mvvm;
using MvvmWorkshop.Shared;

namespace WpfApplication1
{
    public class ViewPersonViewModel : TypedViewModelBase<PersonCargo>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int BirthYear { get; set; }

        protected override void OnInitialize()
        {
            FirstName = Model.FirstName;
            LastName = Model.LastName;
            BirthYear = Model.BirthYear;
        }
    }
}