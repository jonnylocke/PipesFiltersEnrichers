using System;
using System.Collections.Generic;
using PipesFiltersEnrichers.Impl.Filters;
using PipesFiltersEnrichers.Models;

namespace PipesFiltersEnrichers.Impl.Enrichers
{
    internal class InitialiseData : FilterBase<MyResultObj>
    {
        private readonly IEnumerable<Patient> _data;

        public InitialiseData(IEnumerable<Patient> data)
        {
            _data = data;
        }

        protected override bool IsApplicable(MyResultObj input)
        {
            return true;
        }

        protected override MyResultObj Process(MyResultObj input)
        {
            if (!IsApplicable(input))
                return input;

            base.ProcessStartTime = DateTime.Now;
            input.InboundData = _data;
            input.OutboundData = _data;
            base.SetInboundTotal(input);
            base.StopProcessTime(input, "InitialiseData");

            return input;
        }
    }
}