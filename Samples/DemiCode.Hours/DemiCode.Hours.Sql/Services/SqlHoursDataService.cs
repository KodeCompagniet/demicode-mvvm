using System;
using DemiCode.Hours.Shared.Services;
using System.Collections.Generic;
using DemiCode.Hours.Shared.Model;
using System.Data.SqlClient;
using System.Linq;

namespace DemiCode.Hours.Sql.Services
{
    public class SqlHoursDataService : IHoursDataService
    {
        private readonly SqlConnectionFactory _sqlConnectionFactory;

        /// <summary>
        /// Constructs a new SqlHoursDataService using the specified <see cref="SqlConnection"/>.
        /// </summary>
        /// <param name="sqlConnectionFactory">The <see cref="SqlConnectionFactory"/> to use.</param>
        public SqlHoursDataService(SqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public IEnumerable<EmployeeCargo> GetEmployees()
        {
            using (var dc = GetDataContext())
            {
                var query = from employee in dc.Employees
                            select employee.ToCargo();

                return query.ToArray();
            }
        }

        public EmployeeCargo GetEmployee(int id)
        {
            using (var dc = GetDataContext())
            {
                var query = from employee in dc.Employees
                            where employee.Id == id
                            select employee.ToCargo();

                return query.FirstOrDefault();
            }
        }

        public EmployeeCargo GetEmployee(string userName)
        {
            using (var dc = GetDataContext())
            {
                var query = from employee in dc.Employees
                            where employee.UserName == userName
                            select employee.ToCargo();

                return query.FirstOrDefault();
            }
        }

        public EmployeeCargo GetCurrentEmployee()
        {
            var userName = Environment.UserDomainName + "\\" + Environment.UserName;
            return GetEmployee(userName);
        }

        public void StoreEmployee(EmployeeCargo cargo)
        {
            using (var dc = GetDataContext())
            {
                if (cargo.Id.HasValue)
                {
                    var query = from e in dc.Employees
                                where e.Id == cargo.Id.Value
                                select e;
                    var employee = query.First();
                    employee.FirstName = cargo.FirstName;
                    employee.LastName = cargo.LastName;
                    employee.Title = cargo.Title;
                }
                else
                {
                    var employee = new Employee
                    {
                        FirstName = cargo.FirstName,
                        LastName = cargo.LastName,
                        Title = cargo.Title
                    };
                    dc.Employees.InsertOnSubmit(employee);
                }

                dc.SubmitChanges();
            }
        }

        public IEnumerable<ProjectCargo> GetProjects()
        {
            using (var dc = GetDataContext())
            {
                var query = from project in dc.Projects
                            select project.ToCargo();

                return query.ToArray();
            }
        }

        public ProjectCargo GetProject(int id)
        {
            using (var dc = GetDataContext())
            {
                var query = from project in dc.Projects
                            where project.Id == id
                            select project.ToCargo();

                return query.FirstOrDefault();
            }
        }

        public void StoreProject(ProjectCargo cargo)
        {
            using (var dc = GetDataContext())
            {
                //StoreEmployee(cargo.Manager);

                if (cargo.Id.HasValue)
                {
                    var query = from p in dc.Projects
                                where p.Id == cargo.Id.Value
                                select p;
                    var project = query.First();
                    project.Name = cargo.Name;
                    project.ManagerId = cargo.Manager.Id.Value;
                }
                else
                {
                    var project = new Project
                    {
                        Name = cargo.Name,
                        ManagerId = cargo.Manager.Id.Value
                    };
                    dc.Projects.InsertOnSubmit(project);
                }

                dc.SubmitChanges();
            }
        }

        public IEnumerable<WorkItemCargo> GetWorkItemsForEmployee(int employeeId)
        {
            using (var dc = GetDataContext())
            {
                var query = from workItem in dc.WorkItems
                            where workItem.EmployeeId == employeeId
                            select workItem.ToCargo();

                return query.ToArray();
            }
        }

        public IEnumerable<WorkItemCargo> GetWorkItemsForProject(int projectId)
        {
            using (var dc = GetDataContext())
            {
                var query = from workItem in dc.WorkItems
                            where workItem.ProjectId == projectId
                            select workItem.ToCargo();

                return query.ToArray();
            }
        }

        public WorkItemCargo GetWorkItem(int id)
        {
            using (var dc = GetDataContext())
            {
                var query = from workItem in dc.WorkItems
                            where workItem.Id == id
                            select workItem.ToCargo();

                return query.FirstOrDefault();
            }
        }

        public void StoreWorkItem(WorkItemCargo cargo)
        {
            using (var dc = GetDataContext())
            {
                //StoreEmployee(cargo.Employee);
                //StoreProject(cargo.Project);

                if (cargo.Id.HasValue)
                {
                    var query = from p in dc.WorkItems
                                where p.Id == cargo.Id.Value
                                select p;
                    var workItem = query.First();
                    workItem.EmployeeId = cargo.Employee.Id.Value;
                    workItem.ProjectId = cargo.Project.Id.Value;
                    workItem.StartTime = cargo.StartTime;
                    workItem.EndTime = cargo.EndTime;
                    workItem.Comments = cargo.Comments;
                }
                else
                {
                    var workItem = new WorkItem
                    {
                        EmployeeId = cargo.Employee.Id.Value,
                        ProjectId = cargo.Project.Id.Value,
                        StartTime = cargo.StartTime,
                        EndTime = cargo.EndTime,
                        Comments = cargo.Comments
                    };
                    dc.WorkItems.InsertOnSubmit(workItem);
                }

                dc.SubmitChanges();
            }
        }

        #region Helpers

        /// <summary>
        /// Gets a connected <see cref="HoursDataContext"/>.
        /// </summary>
        /// <returns>An <see cref="HoursDataContext"/>.</returns>
        private HoursDataContext GetDataContext()
        {
            var sqlConnection = _sqlConnectionFactory();
            return new HoursDataContext(sqlConnection);
        }

        #endregion
    }
}
