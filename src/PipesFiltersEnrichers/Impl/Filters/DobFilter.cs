using System;
using System.Collections.Generic;
using System.Linq;
using PipesFiltersEnrichers.Models;

namespace PipesFiltersEnrichers.Impl.Filters
{
    internal class DobFilter : FilterBase<MyResultObj>
    {
        protected override bool IsApplicable(MyResultObj input)
        {
            return input.InboundData.Any(x => x.DateOfBirth.Year >= 2000);
        }

        protected override MyResultObj Process(MyResultObj input)
        {
            if (!IsApplicable(input)) return input;

            base.SetInboundTotal(input);
            base.ProcessStartTime = DateTime.Now;

            List<Patient> newOutput = null;
            newOutput.AddRange(input.InboundData.Where(patient => patient.DateOfBirth.Year >= 2000));

            input.OutboundData = newOutput;
            base.StopProcessTime(input, "DobFilter");

            return input;
        }
    }
}