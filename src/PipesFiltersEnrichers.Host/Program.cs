using PipesFiltersEnrichers.Impl;

namespace PipesFiltersEnrichers.Host
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var example = new Example(new Dal());
            example.Run();
        }
    }
}
