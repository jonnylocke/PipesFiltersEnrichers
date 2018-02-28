using System;
using PipesFiltersEnrichers.Impl.Filters;
using PipesFiltersEnrichers.Interfaces;

namespace PipesFiltersEnrichers.Impl.Enrichers
{
    internal class NationalInsuranceEnricher : FilterBase<MyResultObj>
    {
        private readonly IDal _dal;

        public NationalInsuranceEnricher(IDal dal)
        {
            _dal = dal;
        }

        protected override bool IsApplicable(MyResultObj input)
        {
            throw new NotImplementedException();
        }

        protected override MyResultObj Process(MyResultObj input)
        {
            base.SetInboundTotal(input);
            base.ProcessStartTime = DateTime.Now;

            foreach (var patient in input.OutboundData)
                patient.NationalInsuranceNumber =
                    _dal.GetNationalInsuranceId(patient.Forename, patient.Surname, patient.DateOfBirth);

            base.StopProcessTime(input, "NationalInsuranceEnricher");

            return input;
        }
    }
}