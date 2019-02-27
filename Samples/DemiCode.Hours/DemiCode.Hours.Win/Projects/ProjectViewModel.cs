using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using DemiCode.Mvvm;
using DemiCode.Hours.Shared.Model;
using DemiCode.Hours.Shared.Services;
using System.Windows.Input;

namespace DemiCode.Hours.Win.Projects
{
    public class ProjectViewModel : TypedViewModelBase<ProjectCargo>
    {
        private readonly IHoursDataService _hoursDataService;

        public ProjectViewModel(IHoursDataService hoursDataService)
        {
            _hoursDataService = hoursDataService;
        }

        protected override void OnInitialize()
        {
        }

        public override void RegisterCommandBindings(ICommandBindingRegistry registry)
        {
            registry.Add(ApplicationCommands.Save, Save, CanSave);
        }

        public string Name
        {
            get { return Model.Name; }
            set
            {
                Model.Name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        public EmployeeCargo Manager
        {
            get { return Model.Manager; }
            set
            {
                Model.Manager = value;
                RaisePropertyChanged(() => Manager);
            }
        }

        public bool CanSave()
        {
            return IsValid;
        }

        public void Save()
        {
            _hoursDataService.StoreProject(Model);
        }
    }
}
