using System;
using System.Collections.Generic;
using PipesFiltersEnrichers.Models;

namespace PipesFiltersEnrichers.Interfaces
{
    public interface IDal
    {
        IEnumerable<Patient> GetRawData();
        string GetNationalInsuranceId(string foreName, string lastName, DateTime dob);
    }
}