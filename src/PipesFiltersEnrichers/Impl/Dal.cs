using System;
using System.Collections.Generic;
using PipesFiltersEnrichers.Interfaces;
using PipesFiltersEnrichers.Models;

namespace PipesFiltersEnrichers.Impl
{
    public class Dal : IDal
    {
        public IEnumerable<Patient> GetRawData()
        {
            throw new NotImplementedException();
        }

        public string GetNationalInsuranceId(string foreName, string lastName, DateTime dob)
        {
            throw new NotImplementedException();
        }
    }
}