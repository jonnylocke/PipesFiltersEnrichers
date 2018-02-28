using System;
using System.Collections.Generic;
using System.Linq;
using PipesFiltersEnrichers.Models;

namespace PipesFiltersEnrichers.Impl.Filters
{
    internal class DobFilter : FilterBase<MyResultObj>
    {

        private int YearFilter = 1950;

        protected override bool IsApplicable(MyResultObj input)
        {
            return input.InboundData.Any(x => x.DateOfBirth.Year >= YearFilter);
        }

        protected override MyResultObj Process(MyResultObj input)
        {
            if (!IsApplicable(input))
                return input;

            base.ProcessStartTime = DateTime.Now;
            base.SetInboundTotal(input);

            List<Patient> newOutput = new List<Patient>();
            newOutput.AddRange(input.OutboundData.Where(x => x.DateOfBirth.Year >= YearFilter));

            input.OutboundData = newOutput;
            base.StopProcessTime(input, "DobFilter");

            return input;
        }
    }
}