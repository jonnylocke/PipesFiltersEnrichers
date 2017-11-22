using PipesFiltersEnrichers.Interfaces;

namespace PipesFiltersEnrichers.Filters.Pipes
{
    public class Pipeline<T> : IFilterChain<T>
    {
        private IFilter<T> _root;

        public void Apply(T input)
        {
            _root.Apply(input);
        }

        public IFilterChain<T> Register(IFilter<T> filter)
        {
            if (_root == null)
                _root = filter;
            else
            {
                _root.Register(filter);
            }
            return this;
        }
    }
}
