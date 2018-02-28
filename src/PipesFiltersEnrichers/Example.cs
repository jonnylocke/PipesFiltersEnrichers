using System.Collections.Generic;
using PipesFiltersEnrichers.Impl.Enrichers;
using PipesFiltersEnrichers.Impl.Filters;
using PipesFiltersEnrichers.Impl.Pipes;
using PipesFiltersEnrichers.Interfaces;
using PipesFiltersEnrichers.Models;
using System;

namespace PipesFiltersEnrichers
{
    public class Example
    {
        private readonly IDal _dal;

        public Example(IDal dal)
        {
            _dal = dal;
        }

        public void Run()
        {
            var data = _dal.GetRawData();
            var result = new MyResultObj();
            BuildChain(data).Apply(result);

            foreach (var item in result.OutboundData)
            {
                Console.WriteLine(item.Forename);
                Console.WriteLine(item.Surname);
                Console.WriteLine(item.DateOfBirth);
                Console.WriteLine(item.NhsId);
                Console.WriteLine(item.BloodType);
                Console.WriteLine(item.BirthPlace);
                Console.WriteLine(item.NationalInsuranceNumber);
                Console.ReadLine();
            }
        }

        private IFilterChain<MyResultObj> BuildChain(IEnumerable<Patient> data)
        {
            var pipeline = new Pipeline<MyResultObj>();
            pipeline
                .Register(new InitialiseData(data))
                .Register(new DobFilter())
                .Register(new NationalInsuranceEnricher(_dal));

            return pipeline;
        }
    }
}
