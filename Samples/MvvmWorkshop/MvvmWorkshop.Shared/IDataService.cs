using System.Collections.Generic;

namespace MvvmWorkshop.Shared
{
	public interface IDataService
	{
		IEnumerable<PersonCargo> GetPersons();
	}
}