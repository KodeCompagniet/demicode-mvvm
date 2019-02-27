using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using DemiCode.Mvvm;
using DemiCode.Hours.Shared.Model;
using DemiCode.Hours.Shared.Services;

namespace DemiCode.Hours.Win.Employees
{
    public class EmployeeListViewModel : ViewModelBase
    {
        private readonly IHoursDataService _hoursDataService;

        public EmployeeListViewModel(IHoursDataService hoursDataService)
        {
            _hoursDataService = hoursDataService;

            Employees = new ObservableCollection<EmployeeCargo>(_hoursDataService.GetEmployees());
        }

        public ObservableCollection<EmployeeCargo> Employees { get; private set; }
    }
}
