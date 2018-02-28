using System;
using System.Collections.Generic;
using PipesFiltersEnrichers.Impl;
using PipesFiltersEnrichers.Interfaces;
using PipesFiltersEnrichers.Models;

namespace PipesFiltersEnrichers.Host
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var example = new Example(new Dal(new FakeData()));
            example.Run();
        }
    }

    public class FakeData : IDataProvider
    {
        public IEnumerable<IndentificationReference> GetAllIndentificationReferences()
        {
            return new List<IndentificationReference>
            {
                new IndentificationReference {
                    ForeName = "Hannibal",
                    LastName = "Smith",
                    NiNumber = "HS 123 45",
                    DateOfBirth = DateTime.Parse("1949-01-01")
                },
                new IndentificationReference
                {
                    ForeName = "B.A.",
                    LastName = "Baracus",
                    NiNumber = "HS 678 90",
                    DateOfBirth = DateTime.Parse("1959-01-01"),
                },
                new IndentificationReference
                {
                    ForeName = "Conol",
                    LastName = "Decker",
                    NiNumber = "HS 432 78",
                    DateOfBirth = DateTime.Parse("1945-01-01"),
                }
            };
        }

        public IEnumerable<Patient> GetAllPatients()
        {
            return new List<Patient>
            {
                new Patient
                {
                    Forename = "Hannibal",
                    Surname = "Smith",
                    BirthPlace = "L.A.",
                    BloodType = BloodType.OPositive,
                    DateOfBirth = DateTime.Parse("1949-01-01"),
                    NhsId = Guid.NewGuid()          
                },
                new Patient
                {
                    Forename = "B.A.",
                    Surname = "Baracus",
                    BirthPlace = "L.A.",
                    BloodType = BloodType.ONegative,
                    DateOfBirth = DateTime.Parse("1959-01-01"),
                    NhsId = Guid.NewGuid()
                },
                new Patient
                {
                    Forename = "Conol",
                    Surname = "Decker",
                    BirthPlace = "The US Army",
                    BloodType = BloodType.AbNegative,
                    DateOfBirth = DateTime.Parse("1945-01-01"),
                    NhsId = Guid.NewGuid()
                }
            };
        }
    }
}
