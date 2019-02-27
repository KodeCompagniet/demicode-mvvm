using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DemiCode.Mvvm;
using DemiCode.Hours.Shared.Model;
using DemiCode.Hours.Shared.Services;

namespace DemiCode.Hours.Win.WorkItems
{
    public class WorkItemViewModel : TypedViewModelBase<WorkItemCargo>
    {
        private readonly IHoursDataService _hoursDataService;

        public WorkItemViewModel(IHoursDataService hoursDataService)
        {
            _hoursDataService = hoursDataService;
        }

        protected override void OnInitialize()
        {
        }
    }
}
