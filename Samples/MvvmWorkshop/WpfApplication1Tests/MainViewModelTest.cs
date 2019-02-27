using MvvmWorkshop.Shared;
using NUnit.Framework;
using Rhino.Mocks;
using WpfApplication1;

namespace WpfApplication1Tests
{
	[TestFixture]
	public class MainViewModelTest
	{
		[Test]
		public void Construction_LoadsPersons()
		{
			var dataService = MockRepository.GenerateStub<IDataService>();
			dataService.Stub(x => x.GetPersons()).Return(new[]
			                                             	{
			                                             		new PersonCargo
			                                             			{FirstName = "Petter", LastName = "Pettersen", BirthYear = 1990},
			                                             		new PersonCargo
			                                             			{FirstName = "Nils", LastName = "Karlsen", BirthYear = 1990}
			                                             	});

			var vm = new MainViewModel(dataService);

			Assert.That(vm.Persons.Count, Is.EqualTo(2));
		}


		[Test]
		public void NewPersonTest()
		{
			var dataService = MockRepository.GenerateStub<IDataService>();
			dataService.Stub(x => x.GetPersons()).Return(new PersonCargo[0]);

			var vm = new MainViewModel(dataService);
			vm.NewPerson();

			Assert.That(vm.Persons.Count, Is.EqualTo(1));
		}
	}
}