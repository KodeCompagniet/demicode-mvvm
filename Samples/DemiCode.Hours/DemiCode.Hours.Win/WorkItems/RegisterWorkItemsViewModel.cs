using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DemiCode.Mvvm;
using DemiCode.Hours.Shared.Model;
using DemiCode.Hours.Shared.Services;
using System.Collections.ObjectModel;

namespace DemiCode.Hours.Win.WorkItems
{
    public class RegisterWorkItemsViewModel : ViewModelBase
    {
        private readonly IHoursDataService _hoursDataService;
        private readonly int _employeeId;

        /// <summary>
        /// Construct a view model.
        /// </summary>
        /// <exception cref="InvalidOperationException">If the current user does not have a corresponding employee</exception>
        public RegisterWorkItemsViewModel(IHoursDataService hoursDataService)
        {
            _hoursDataService = hoursDataService;
            WorkItems = new ObservableCollection<WorkItemCargo>();
            var currentEmployee = _hoursDataService.GetCurrentEmployee();
            if (currentEmployee == null)
                throw new InvalidOperationException("Current user does not have an employee");
            _employeeId = currentEmployee.Id.Value;
        }

        public override void Initialize(object model)
        {
            foreach (var workItem in _hoursDataService.GetWorkItemsForEmployee(_employeeId))
                WorkItems.Add(workItem);
        }

        public ObservableCollection<WorkItemCargo> WorkItems { get; private set; }
    }
}
