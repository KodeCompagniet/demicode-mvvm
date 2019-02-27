using System.Collections.Generic;

namespace MvvmWorkshop.Shared
{
	public class DataService : IDataService
	{
	    public DataService(IConnection connection)
	    {
	    }

	    public IEnumerable<PersonCargo> GetPersons()
		{
			return new[]
			       	{
			       		new PersonCargo{FirstName = "Petter", LastName = "Pettersen", BirthYear = 1990}, 
			       		new PersonCargo{FirstName = "Nils", LastName = "Karlsen", BirthYear = 1990}, 
			       		new PersonCargo{FirstName = "Ole", LastName = "Nilsen", BirthYear = 1990}, 
			       		new PersonCargo{FirstName = "Karl", LastName = "Olsen", BirthYear = 1990}, 
			       		new PersonCargo{FirstName = "Bjarne", LastName = "Pettersen", BirthYear = 1990}, 
					};
		}
	}
}