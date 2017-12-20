using System;
using System.Collections.Generic;
using System.Linq;
using PipesFiltersEnrichers.Interfaces;

namespace PipesFiltersEnrichers.Impl.Filters
{
    internal abstract class FilterBase<T> : IFilter<T>
    {
        private IFilter<T> _next;
        private int InboundCount { get; set; }

        protected abstract bool IsApplicable(MyResultObj input);
        protected abstract T Process(T input);
        
        public DateTime ProcessStartTime { get; set; }
        

        public T Apply(T input)
        {
            var val = Process(input);
            if (_next != null)
                val = _next.Apply(val);
            return val;
        }

        public void Register(IFilter<T> filter)
        {
            if (_next == null)
                _next = filter;
            else
                _next.Register(filter);
        }

        public void SetInboundTotal(MyResultObj resultObj)
        {
            InboundCount = resultObj.OutboundData?.Count() ?? resultObj.InboundData.Count();
        }

        public void StopProcessTime(MyResultObj resultObj, string filterName)
        {
            var metaData = new PipelineMetadata
            {
                Filter = filterName,
                FilterDeltaTime = DateTime.Now.Subtract(ProcessStartTime),
                InboundDataCount = InboundCount,
                OutboundDataCount = resultObj.OutboundData?.Count() ?? resultObj.InboundData.Count()
            };
            resultObj.PipelineMetadata.Add(metaData);
        }
    }
}
