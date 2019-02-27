using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using DemiCode.Mvvm;
using DemiCode.Hours.Shared.Model;
using DemiCode.Hours.Shared.Services;
using System.Windows;

namespace DemiCode.Hours.Win
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IHoursDataService _hoursDataService;

        public MainViewModel(IHoursDataService hoursDataService)
        {
            _hoursDataService = hoursDataService;

//            Employees = new ObservableCollection<EmployeeCargo>(_hoursDataService.GetEmployees());
//            Projects = new ObservableCollection<ProjectCargo>(_hoursDataService.GetProjects());
        }

        public override void RegisterCommandBindings(ICommandBindingRegistry registry)
        {
        }

//        public ObservableCollection<EmployeeCargo> Employees { get; private set; }

//        public ObservableCollection<ProjectCargo> Projects { get; private set; }

        public void OpenProjects()
        {
            MessageBox.Show("Hm. How to?");
        }
    }
}
