namespace PipesFiltersEnrichers.Interfaces
{
    public interface IFilterChain<T>
    {
        void Apply(T input);
        IFilterChain<T> Register(IFilter<T> filter);
    }
}