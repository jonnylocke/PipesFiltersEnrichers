using System.Collections.Generic;
using PipesFiltersEnrichers.Impl.Enrichers;
using PipesFiltersEnrichers.Impl.Filters;
using PipesFiltersEnrichers.Impl.Pipes;
using PipesFiltersEnrichers.Interfaces;
using PipesFiltersEnrichers.Models;

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
            BuildChain(data).Apply(new MyResultObj());
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
