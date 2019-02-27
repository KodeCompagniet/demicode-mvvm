using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using DemiCode.Mvvm;
using MvvmWorkshop.Shared;

namespace WpfApplication1
{
	public class MainViewModel : ViewModelBase
	{
		public ObservableCollection<PersonCargo> Persons
		{
			get;
			private set;
		}

		public MainViewModel(IDataService dataService)
		{
			Persons = new ObservableCollection<PersonCargo>(dataService.GetPersons());
		}

		public override void RegisterCommandBindings(ICommandBindingRegistry registry)
		{
			registry.Add(ApplicationCommands.New, NewPerson);
		}

		public void NewPerson()
		{
			Persons.Add(new PersonCargo{FirstName = "Ole", LastName = "Olsen", BirthYear = 1980});
		}
	}
}