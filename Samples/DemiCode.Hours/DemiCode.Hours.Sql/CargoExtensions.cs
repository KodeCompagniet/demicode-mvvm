using DemiCode.Hours.Shared;
using DemiCode.Hours.Shared.Model;

namespace DemiCode.Hours.Sql
{
    public static class CargoExtensions
    {
        public static EmployeeCargo ToCargo(this Employee employee)
        {
            return new EmployeeCargo
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Title = employee.Title
            };
        }

        public static ProjectCargo ToCargo(this Project project)
        {
            return new ProjectCargo
            {
                Id = project.Id,
                Name = project.Name,
                Manager = project.Manager.ToCargo()
            };
        }

        public static WorkItemCargo ToCargo(this WorkItem workItem)
        {
            return new WorkItemCargo
            {
                Id = workItem.Id,
                Employee = workItem.Employee.ToCargo(),
                Project = workItem.Project.ToCargo(),
                StartTime = workItem.StartTime,
                EndTime = workItem.EndTime,
                Comments = workItem.Comments
            };
        }

        public static Employee ToEntity(this EmployeeCargo cargo)
        {
            var employee = new Employee
            {
                FirstName = cargo.FirstName,
                LastName = cargo.LastName,
                Title = cargo.Title
            };
            if (cargo.Id.HasValue)
                employee.Id = cargo.Id.Value;
            return employee;
        }

        public static Project ToEntity(this ProjectCargo cargo)
        {
            var project = new Project
            {
                Name = cargo.Name,
                Manager = cargo.Manager.ToEntity()
            };
            if (cargo.Id.HasValue)
                project.Id = cargo.Id.Value;
            return project;
        }

        public static WorkItem ToEntity(this WorkItemCargo cargo)
        {
            var workItem = new WorkItem
            {
                Employee = cargo.Employee.ToEntity(),
                Project = cargo.Project.ToEntity(),
                StartTime = cargo.StartTime,
                EndTime = cargo.EndTime,
                Comments = cargo.Comments
            };
            if (cargo.Id.HasValue)
                workItem.Id = cargo.Id.Value;
            return workItem;
        }

        public static void UpdateFromCargo(this Employee employee, EmployeeCargo cargo)
        {
            if (cargo.Id.HasValue)
                employee.Id = cargo.Id.Value;
            employee.FirstName = cargo.FirstName;
            employee.LastName = cargo.LastName;
            employee.Title = cargo.Title;
        }

        public static void UpdateFromCargo(this Project project, ProjectCargo cargo)
        {
            if (cargo.Id.HasValue)
                project.Id = cargo.Id.Value;
            project.Name = cargo.Name;
            project.Manager = cargo.Manager.ToEntity();
        }

        public static void UpdateFromCargo(this WorkItem workItem, WorkItemCargo cargo)
        {
            if (cargo.Id.HasValue)
                workItem.Id = cargo.Id.Value;
            workItem.Employee = cargo.Employee.ToEntity();
            workItem.Project = cargo.Project.ToEntity();
            workItem.StartTime = cargo.StartTime;
            workItem.EndTime = cargo.EndTime;
            workItem.Comments = cargo.Comments;
        }
    }
}
