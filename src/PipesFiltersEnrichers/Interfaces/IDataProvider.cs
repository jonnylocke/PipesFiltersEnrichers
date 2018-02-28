using PipesFiltersEnrichers.Models;
using System.Collections.Generic;

namespace PipesFiltersEnrichers.Interfaces
{
    public interface IDataProvider
    {
        IEnumerable<Patient> GetAllPatients();
        IEnumerable<IndentificationReference> GetAllIndentificationReferences();
    }
}
