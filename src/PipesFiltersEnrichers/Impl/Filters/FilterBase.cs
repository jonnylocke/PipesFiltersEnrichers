using System;
using System.Collections.Generic;
using System.Linq;
using PipesFiltersEnrichers.Filters.Interfaces;

namespace PipesFiltersEnrichers.Filters
{
    public abstract class FilterBase<T> : IFilter<T>
    {
        private IFilter<T> _next;
        private int InboundNumberOfKeys { get; set; }

        protected abstract T Process(T value);
        protected abstract bool IsApplicable(SearchResult value);
        public DateTime ProcessStartTime { get; set; }
        

        public T Apply(T value)
        {
            var val = Process(value);
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

        public void SetInboundKeysTotal(SearchResult searchResult)
        {
            InboundNumberOfKeys = searchResult.Output?.Count() ?? searchResult.Data.Count();
        }

        public void StopProcessTime(SearchResult searchResult, string filterName)
        {
            var metaData = new FilterMetaData
            {
                Filter = filterName,
                FilterDeltaTime = DateTime.Now.Subtract(ProcessStartTime),
                InboundNumberOfKeys = InboundNumberOfKeys,
                OutboundNumberOfKeys = searchResult.Output?.Count() ?? searchResult.Data.Count()
            };
            searchResult.FilterMetaData.Add(metaData);
        }

        protected void RemoveKeysFromOutput(SearchResult value, List<ResourceComposite> resources)
        {
            if(!resources.Any()) return;

            var temp = value.Output.ToList();

            foreach (var resource in resources)
                temp.Remove(resource);

            value.Output = temp.AsEnumerable();
        }

        protected void RemoveKeysFromData(SearchResult value, List<ResourceComposite> resources)
        {
            var temp = value.Data.ToList();

            foreach (var resource in resources)
                temp.Remove(resource);

            value.Data = temp.AsEnumerable();
        }
    }
}
