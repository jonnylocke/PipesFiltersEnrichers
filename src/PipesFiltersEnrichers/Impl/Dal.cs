using System;
using System.Collections.Generic;
using PipesFiltersEnrichers.Interfaces;
using PipesFiltersEnrichers.Models;
using System.Linq;

namespace PipesFiltersEnrichers.Impl
{
    public class Dal : IDal
    {
        IDataProvider _dataProvider;
        IEnumerable<IndentificationReference> _indentificaitonReferences;

        public Dal(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
            _indentificaitonReferences = dataProvider.GetAllIndentificationReferences();
        }

        public IEnumerable<Patient> GetRawData()
        {
            return _dataProvider.GetAllPatients();
        }

        public string GetNationalInsuranceId(string foreName, string lastName, DateTime dob)
        {
            var match = _indentificaitonReferences.FirstOrDefault(
                x => x.ForeName.ToLower() == foreName.ToLower() &&
                x.LastName.ToLower() == lastName.ToLower() &&
                x.DateOfBirth.ToShortDateString().Equals(dob.ToShortDateString()));

            return match.NiNumber;
        }
    }
}