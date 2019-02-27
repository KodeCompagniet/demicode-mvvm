using System;

namespace DemiCode.Hours.Shared.Model
{
    public class WorkItemCargo
    {
        public int? Id { get; set; }
        public EmployeeCargo Employee { get; set; }
        public ProjectCargo Project { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Comments { get; set; }
    }
}
