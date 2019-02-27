using System.Collections.Generic;
using DemiCode.Hours.Shared.Model;

namespace DemiCode.Hours.Shared.Services
{
    /// <summary>
    /// The interface for the Hours Data Service.
    /// </summary>
    public interface IHoursDataService
    {
        /// <summary>
        /// Gets a list of employees.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="EmployeeCargo">employees</see>.</returns>
        IEnumerable<EmployeeCargo> GetEmployees();

        /// <summary>
        /// Gets an employee.
        /// </summary>
        /// <param name="id">The Id if the employee.</param>
        /// <returns>The <see cref="EmployeeCargo">employee</see> with the specified Id.</returns>
        EmployeeCargo GetEmployee(int id);

        /// <summary>
        /// Gets the <see cref="EmployeeCargo"/> for the specified user name.
        /// </summary>
        /// <param name="userName">The full user name (domain\username).</param>
        /// <returns>The <see cref="EmployeeCargo"/> for the specified user name, or <c>null</c> if none exists.</returns>
        EmployeeCargo GetEmployee(string userName);

        /// <summary>
        /// Gets the <see cref="EmployeeCargo"/> for the current user.
        /// </summary>
        /// <returns>The <see cref="EmployeeCargo"/> for the current user, or <c>null</c> if none exists.</returns>
        EmployeeCargo GetCurrentEmployee();

        /// <summary>
        /// Creates or updates an employee.
        /// </summary>
        /// <param name="employee">The <see cref="EmployeeCargo">employee</see> to store.</param>
        void StoreEmployee(EmployeeCargo employee);

        /// <summary>
        /// Gets a list of projects.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="ProjectCargo">projects</see>.</returns>
        IEnumerable<ProjectCargo> GetProjects();

        /// <summary>
        /// Gets a project.
        /// </summary>
        /// <param name="id">The Id if the project.</param>
        /// <returns>The <see cref="ProjectCargo">project</see> with the specified Id.</returns>
        ProjectCargo GetProject(int id);

        /// <summary>
        /// Creates or updates a project.
        /// </summary>
        /// <param name="project">The <see cref="ProjectCargo">project</see> to store.</param>
        void StoreProject(ProjectCargo project);

        /// <summary>
        /// Gets a list of work items for the specified employee.
        /// </summary>
        /// <param name="employeeId">The employee Id.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="WorkItemCargo">work items</see>.</returns>
        IEnumerable<WorkItemCargo> GetWorkItemsForEmployee(int employeeId);

        /// <summary>
        /// Gets a list of work items for the specified project.
        /// </summary>
        /// <param name="projectId">The project Id.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="WorkItemCargo">work items</see>.</returns>
        IEnumerable<WorkItemCargo> GetWorkItemsForProject(int projectId);

        /// <summary>
        /// Gets a work item.
        /// </summary>
        /// <param name="id">The Id if the work item.</param>
        /// <returns>The <see cref="WorkItemCargo">work item</see> with the specified Id.</returns>
        WorkItemCargo GetWorkItem(int id);

        /// <summary>
        /// Creates or updates a work item.
        /// </summary>
        /// <param name="employee">The <see cref="WorkItemCargo">work item</see> to store.</param>
        void StoreWorkItem(WorkItemCargo workItem);
    }
}
