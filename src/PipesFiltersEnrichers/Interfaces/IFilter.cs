using System;

namespace PipesFiltersEnrichers.Interfaces
{
    public interface IFilter<T>
    {
        T Apply(T input);
        void Register(IFilter<T> filter);
    //    DateTime ProcessStartTime { get; set; }
    //    void SetInboundKeysTotal(SearchResult searchResult);
    //    void StopProcessTime(SearchResult searchResult, string filterName);
    }
}
