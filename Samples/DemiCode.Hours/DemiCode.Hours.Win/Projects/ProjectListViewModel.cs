using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using DemiCode.Mvvm;
using DemiCode.Hours.Shared.Model;
using DemiCode.Hours.Shared.Services;

namespace DemiCode.Hours.Win.Projects
{
    public class ProjectListViewModel : ViewModelBase
    {
        private readonly IHoursDataService _hoursDataService;

        public ProjectListViewModel(IHoursDataService hoursDataService)
        {
            _hoursDataService = hoursDataService;

            Projects = new ObservableCollection<ProjectCargo>(_hoursDataService.GetProjects());
        }

        public ObservableCollection<ProjectCargo> Projects { get; private set; }
    }
}
