﻿using System;

namespace PipesFiltersEnrichers.Models
{
    public class Patient
    {
        public Guid NhsId { get; set; }
        public string NationalInsuranceNumber { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public BloodType BloodType { get; set; }
        public string BirthPlace { get; set; }
    }

    public class IndentificationReference
    {
        public string ForeName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string NiNumber { get; set; }
    }

    public enum BloodType
    {
        ONegative,
        OPositive,
        ANegative,
        APositive,
        BNegative,
        BPositive,
        AbNegative,
        AbPositive
    }
}