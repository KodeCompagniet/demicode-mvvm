using System;
using MvvmWorkshop.Shared;
using NUnit.Framework;
using WpfApplication1;

namespace WpfApplication1Tests
{
    [TestFixture]
    public class EditPersonViewModelTest
    {
        [Test]
        public void Construct()
        {
            var person = new PersonCargo {FirstName = "Ole", LastName = "Bull", BirthYear = 1834};
            var vm = new EditPersonViewModel();
            vm.Initialize(person);

            Assert.That(vm.FirstName, Is.EqualTo("Ole"));
            Assert.That(vm.LastName, Is.EqualTo("Bull"));
            Assert.That(vm.BirthYear, Is.EqualTo(1834));
        }
    }
}
