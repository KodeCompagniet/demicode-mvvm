using NUnit.Framework;
using System.Data.SqlClient;
using System.Configuration;

using System;
using DemiCode.Hours.Shared.Model;

namespace DemiCode.Hours.Sql.Test
{
    [TestFixture]
    public class CargoExtensionsTests
    {
        [Test]
        public void EmployeeToCargo_ReturnsEmployeeCargo()
        {
            var employee = new Employee { Id = 1, FirstName = "Arjan", LastName = "Einbu", Title = "Manager" };

            var cargo = employee.ToCargo();

            Assert.That(cargo, Is.Not.Null);
            Assert.That(cargo.Id, Is.EqualTo(1));
            Assert.That(cargo.FirstName, Is.EqualTo("Arjan"));
            Assert.That(cargo.LastName, Is.EqualTo("Einbu"));
            Assert.That(cargo.Title, Is.EqualTo("Manager"));
        }

        [Test]
        public void ProjectToCargo_ReturnsProjectCargo()
        {
            var manager = new Employee { Id = 1, FirstName = "Peter", LastName = "Lillevold", Title = "System Developer" };
            var project = new Project { Id = 1, Name = "4Subsea Wellhead", Manager = manager };

            var cargo = project.ToCargo();

            Assert.That(cargo, Is.Not.Null);
            Assert.That(cargo.Id, Is.EqualTo(1));
            Assert.That(cargo.Name, Is.EqualTo("4Subsea Wellhead"));
            Assert.That(cargo.Manager, Is.Not.Null);
            Assert.That(cargo.Manager.Id, Is.EqualTo(1));
            Assert.That(cargo.Manager.FirstName, Is.EqualTo("Peter"));
            Assert.That(cargo.Manager.LastName, Is.EqualTo("Lillevold"));
            Assert.That(cargo.Manager.Title, Is.EqualTo("System Developer"));
        }

        [Test]
        public void WorkItemToCargo_ReturnsWorkItemCargo()
        {
            var employee = new Employee { Id = 4, FirstName = "Tor Kristen", LastName = "Haugen", Title = "System Developer" };
            var manager = new Employee { Id = 1, FirstName = "Peter", LastName = "Lillevold", Title = "System Developer" };
            var project = new Project { Id = 1, Name = "4Subsea Wellhead", Manager = manager };
            var workItem = new WorkItem { Id = 1, Employee = employee, Project = project, StartTime = new DateTime(2010, 1, 4, 9, 0, 0), EndTime = new DateTime(2010, 1, 4, 12, 30, 0), Comments = "Skrev litt kode" };

            var cargo = workItem.ToCargo();

            Assert.That(cargo, Is.Not.Null);
            Assert.That(cargo.Id, Is.EqualTo(1));
            Assert.That(cargo.Employee, Is.Not.Null);
            Assert.That(cargo.Employee.Id, Is.EqualTo(4));
            Assert.That(cargo.Employee.FirstName, Is.EqualTo("Tor Kristen"));
            Assert.That(cargo.Employee.LastName, Is.EqualTo("Haugen"));
            Assert.That(cargo.Employee.Title, Is.EqualTo("System Developer"));
            Assert.That(cargo.Project, Is.Not.Null);
            Assert.That(cargo.Project.Id, Is.EqualTo(1));
            Assert.That(cargo.Project.Name, Is.EqualTo("4Subsea Wellhead"));
            Assert.That(cargo.Project.Manager, Is.Not.Null);
            Assert.That(cargo.Project.Manager.Id, Is.EqualTo(1));
            Assert.That(cargo.StartTime, Is.EqualTo(new DateTime(2010, 1, 4, 9, 0, 0)));
            Assert.That(cargo.EndTime, Is.EqualTo(new DateTime(2010, 1, 4, 12, 30, 0)));
            Assert.That(cargo.Comments, Is.EqualTo("Skrev litt kode"));
        }

        [Test]
        public void EmployeeToEntity_ReturnsEmployeeEntity()
        {
            var cargo = new EmployeeCargo { Id = 4, FirstName = "Tor Kristen", LastName = "Haugen", Title = "System Developer" };

            var entity = cargo.ToEntity();

            Assert.That(entity, Is.Not.Null);
            Assert.That(entity.Id, Is.EqualTo(4));
            Assert.That(entity.FirstName, Is.EqualTo("Tor Kristen"));
            Assert.That(entity.LastName, Is.EqualTo("Haugen"));
            Assert.That(entity.Title, Is.EqualTo("System Developer"));
        }

        [Test]
        public void ProjectToEntity_ReturnsProjectEntity()
        {
            var managerCargo = new EmployeeCargo { Id = 4, FirstName = "Tor Kristen", LastName = "Haugen", Title = "System Developer" };
            var cargo = new ProjectCargo { Id = 2, Name = "KC MVVM", Manager = managerCargo };

            var entity = cargo.ToEntity();

            Assert.That(entity, Is.Not.Null);
            Assert.That(entity.Id, Is.EqualTo(2));
            Assert.That(entity.Name, Is.EqualTo("KC MVVM"));
            Assert.That(entity.Manager, Is.Not.Null);
            Assert.That(entity.Manager.FirstName, Is.EqualTo("Tor Kristen"));
        }

        [Test]
        public void WorkItemToEntity_ReturnsWorkItemEntity()
        {
            var employeeCargo = new EmployeeCargo { Id = 4, FirstName = "Tor Kristen", LastName = "Haugen", Title = "System Developer" };
            var managerCargo = new EmployeeCargo { Id = 2, FirstName = "Peter", LastName = "Lillevold", Title = "System Developer" };
            var projectCargo = new ProjectCargo { Id = 1, Name = "4Subsea Wellhead", Manager = managerCargo };
            var cargo = new WorkItemCargo { Id = 17, Employee = employeeCargo, Project = projectCargo, StartTime = new DateTime(2009, 12, 3, 9, 0, 0), EndTime = new DateTime(2009, 12, 3, 12, 30, 0), Comments = "Skrev litt kode" };

            var entity = cargo.ToEntity();

            Assert.That(entity, Is.Not.Null);
            Assert.That(entity.Id, Is.EqualTo(17));
            Assert.That(entity.Employee, Is.Not.Null);
            Assert.That(entity.Employee.FirstName, Is.EqualTo("Tor Kristen"));
            Assert.That(entity.Project, Is.Not.Null);
            Assert.That(entity.Project.Name, Is.EqualTo("4Subsea Wellhead"));
            Assert.That(entity.StartTime, Is.EqualTo(new DateTime(2009, 12, 3, 9, 0, 0)));
            Assert.That(entity.EndTime, Is.EqualTo(new DateTime(2009, 12, 3, 12, 30, 0)));
            Assert.That(entity.Comments, Is.EqualTo("Skrev litt kode"));
        }

        [Test]
        public void UpdateEmployeeEntity_SetsPropertiesFromCargo()
        {
            var cargo = new EmployeeCargo { Id = 4, FirstName = "Tor Kristen", LastName = "Haugen", Title = "System Developer" };
            var entity = new Employee { Id = 4, FirstName = "Tor", LastName = "Haguen", Title = "Cooffe boy" };

            entity.UpdateFromCargo(cargo);

            Assert.That(entity, Is.Not.Null);
            Assert.That(entity.Id, Is.EqualTo(4));
            Assert.That(entity.FirstName, Is.EqualTo("Tor Kristen"));
            Assert.That(entity.LastName, Is.EqualTo("Haugen"));
            Assert.That(entity.Title, Is.EqualTo("System Developer"));
        }

        [Test]
        public void UpdateProjectEntity_SetsPropertiesFromCargo()
        {
            var managerCargo = new EmployeeCargo { Id = 4, FirstName = "Tor Kristen", LastName = "Haugen", Title = "System Developer" };
            var cargo = new ProjectCargo { Id = 2, Name = "DemiCode MVVM", Manager = managerCargo };
            var manager = new Employee { Id = 2, FirstName = "Peter", LastName = "Lillevold", Title = "System Developer" };
            var entity = new Project { Id = 2, Name = "KC MVVM" };

            entity.UpdateFromCargo(cargo);

            Assert.That(entity, Is.Not.Null);
            Assert.That(entity.Id, Is.EqualTo(2));
            Assert.That(entity.Name, Is.EqualTo("DemiCode MVVM"));
            Assert.That(entity.Manager, Is.Not.Null);
            Assert.That(entity.Manager.FirstName, Is.EqualTo("Tor Kristen"));
        }

        [Test]
        public void UpdateWorkItemEntity_SetsPropertiesFromCargo()
        {
            var employeeCargo = new EmployeeCargo { Id = 2, FirstName = "Peter", LastName = "Lillevold", Title = "System Developer" };
            var managerCargo = new EmployeeCargo { Id = 4, FirstName = "Tor Kristen", LastName = "Haugen", Title = "System Developer" };
            var projectCargo = new ProjectCargo { Id = 2, Name = "DemiCode MVVM", Manager = managerCargo };
            var cargo = new WorkItemCargo { Id = 37, Employee = employeeCargo, Project = projectCargo, StartTime = new DateTime(2010, 1, 15, 11, 0, 0), EndTime = new DateTime(2010, 1, 15, 18, 7, 0), Comments = "Oppdaterte enhetstester" };
            var employee = new Employee { Id = 4, FirstName = "Tor Kristen", LastName = "Haugen", Title = "System Developer" };
            var manager = new Employee { Id = 2, FirstName = "Peter", LastName = "Lillevold", Title = "System Developer" };
            var project = new Project { Id = 1, Name = "4Subsea Wellhead", Manager = manager };
            var entity = new WorkItem { Id = 17, Employee = employee, Project = project, StartTime = new DateTime(2009, 12, 3, 9, 0, 0), EndTime = new DateTime(2009, 12, 3, 12, 30, 0), Comments = "Skrev litt kode" };

            entity.UpdateFromCargo(cargo);

            Assert.That(entity, Is.Not.Null);
            Assert.That(entity.Id, Is.EqualTo(37));
            Assert.That(entity.Employee, Is.Not.Null);
            Assert.That(entity.Employee.FirstName, Is.EqualTo("Peter"));
            Assert.That(entity.Project, Is.Not.Null);
            Assert.That(entity.Project.Name, Is.EqualTo("DemiCode MVVM"));
            Assert.That(entity.StartTime, Is.EqualTo(new DateTime(2010, 1, 15, 11, 0, 0)));
            Assert.That(entity.EndTime, Is.EqualTo(new DateTime(2010, 1, 15, 18, 7, 0)));
            Assert.That(entity.Comments, Is.EqualTo("Oppdaterte enhetstester"));
        }
    }
}
