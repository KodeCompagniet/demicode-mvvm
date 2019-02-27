using System.Linq;
using NUnit.Framework;
using System.Data.SqlClient;
using System.Configuration;

using System;

namespace DemiCode.Hours.Sql.Services.Test
{
    [TestFixture]
    public class SqlHoursDataServiceTests
    {
        SqlConnectionFactory _connectionFactory;

        [SetUp]
        public void SetUp()
        {
            // TODO: Run SQL Create/populate scripts

            string connectionString = ConfigurationManager.ConnectionStrings["DemiCode.Hours.Sql.Properties.Settings.HoursConnectionString"].ConnectionString;
            _connectionFactory = () => new SqlConnection(connectionString);
        }

        [Test, Category("Integration")]
        public void GetEmployees_ReturnsMultiple()
        {
            SqlHoursDataService service = new SqlHoursDataService(_connectionFactory);

            var employees = service.GetEmployees();

            Assert.That(employees, Is.Not.Null);
            Assert.That(employees.Count(), Is.GreaterThan(1));
        }

        [Test, Category("Integration")]
        public void GetEmployee_ById_ReturnsOne()
        {
            SqlHoursDataService service = new SqlHoursDataService(_connectionFactory);

            var employee = service.GetEmployee(1);

            Assert.That(employee, Is.Not.Null);
            Assert.That(employee.Id, Is.EqualTo(1));
            Assert.That(employee.FirstName, Is.EqualTo("Arjan"));
            Assert.That(employee.LastName, Is.EqualTo("Einbu"));
            Assert.That(employee.Title, Is.EqualTo("Manager"));
        }

        [Test, Category("Integration")]
        public void GetEmployee_ByUserName_ReturnsOne()
        {
            SqlHoursDataService service = new SqlHoursDataService(_connectionFactory);

            var employee = service.GetEmployee(@"CORP\arjan");

            Assert.That(employee, Is.Not.Null);
            Assert.That(employee.Id, Is.EqualTo(1));
            Assert.That(employee.FirstName, Is.EqualTo("Arjan"));
            Assert.That(employee.LastName, Is.EqualTo("Einbu"));
            Assert.That(employee.Title, Is.EqualTo("Manager"));
        }

        [Test, Category("Integration")]
        public void GetProjects_ReturnsMultiple()
        {
            SqlHoursDataService service = new SqlHoursDataService(_connectionFactory);

            var projects = service.GetProjects();

            Assert.That(projects, Is.Not.Null);
            Assert.That(projects.Count(), Is.GreaterThan(1));
        }

        [Test, Category("Integration")]
        public void GetProject_ReturnsOne()
        {
            SqlHoursDataService service = new SqlHoursDataService(_connectionFactory);

            var project = service.GetProject(1);

            Assert.That(project, Is.Not.Null);
            Assert.That(project.Id, Is.EqualTo(1));
            Assert.That(project.Name, Is.EqualTo("4Subsea Wellhead"));
            Assert.That(project.Manager, Is.Not.Null);
            Assert.That(project.Manager.Id, Is.EqualTo(2));
        }

        [Test, Category("Integration")]
        public void GetWorkItemsForEmployee_ReturnsMultiple()
        {
            SqlHoursDataService service = new SqlHoursDataService(_connectionFactory);

            var workItems = service.GetWorkItemsForEmployee(4);

            Assert.That(workItems, Is.Not.Null);
            Assert.That(workItems.Count(), Is.GreaterThan(1));
        }

        [Test, Category("Integration")]
        public void GetWorkItemsForProject_ReturnsMultiple()
        {
            SqlHoursDataService service = new SqlHoursDataService(_connectionFactory);

            var workItems = service.GetWorkItemsForProject(2);

            Assert.That(workItems, Is.Not.Null);
            Assert.That(workItems.Count(), Is.GreaterThan(1));
        }

        [Test, Category("Integration")]
        public void GetWorkItem_ReturnsOne()
        {
            SqlHoursDataService service = new SqlHoursDataService(_connectionFactory);

            var workItem = service.GetWorkItem(1);

            Assert.That(workItem, Is.Not.Null);
            Assert.That(workItem.Id, Is.EqualTo(1));
            Assert.That(workItem.Employee, Is.Not.Null);
            Assert.That(workItem.Employee.Id, Is.EqualTo(4));
            Assert.That(workItem.Project, Is.Not.Null);
            Assert.That(workItem.Project.Id, Is.EqualTo(2));
            Assert.That(workItem.StartTime, Is.EqualTo(new DateTime(2010, 1, 4, 9, 0, 0)));
            Assert.That(workItem.EndTime, Is.EqualTo(new DateTime(2010, 1, 4, 12, 0, 0)));
            Assert.That(workItem.Comments, Is.EqualTo("Start MVVM"));
        }
    }
}
