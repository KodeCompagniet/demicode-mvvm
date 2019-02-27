using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using DemiCode.Mvvm;
using DemiCode.Hours.Shared.Model;
using DemiCode.Hours.Shared.Services;
using System.Windows.Input;

namespace DemiCode.Hours.Win.Employees
{
    public class EmployeeViewModel : ViewModelBase
    {
        private readonly IHoursDataService _hoursDataService;
        private EmployeeCargo _cargo;

        public EmployeeViewModel(IHoursDataService hoursDataService)
        {
            _hoursDataService = hoursDataService;
        }

        public override void Initialize(object model)
        {
            _cargo = model as EmployeeCargo;
        }

        public override void RegisterCommandBindings(ICommandBindingRegistry registry)
        {
            registry.Add(ApplicationCommands.Save, Save, CanSave);
        }

        public string FirstName
        {
            get { return _cargo.FirstName; }
            set
            {
                _cargo.FirstName = value;
                RaisePropertyChanged(() => FirstName);
            }
        }

        public string LastName
        {
            get { return _cargo.LastName; }
            set
            {
                _cargo.LastName = value;
                RaisePropertyChanged(() => LastName);
            }
        }

        public string Title
        {
            get { return _cargo.Title; }
            set
            {
                _cargo.Title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        public bool CanSave()
        {
            return IsValid;
        }

        public void Save()
        {
            _hoursDataService.StoreEmployee(_cargo);
        }
    }
}
