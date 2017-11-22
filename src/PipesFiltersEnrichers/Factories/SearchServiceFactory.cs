using System.Collections.Generic;
using System.Linq;
using PipesFiltersEnrichers.Impl;

namespace Iglu.PlanetCruise.Translations.BusinessLogic.Factories
{
    public class SearchServiceFactory : ISearchFactory
    {
        private readonly ICacheManager _cacheManager;
        private readonly IResourceRepository _resourceRepository;
        private readonly ILanguageService _languageService;
        private readonly IResourceData _resourceData;

        private readonly IList<IMoveResource> _moveResourceOptions;
        private readonly IList<IRemovalFilter> _removalFilters;

        public SearchServiceFactory(ICacheManager cacheManager, IResourceRepository resourceRepository, ILanguageService languageService, IResourceData resourceData)
        {
            _cacheManager = cacheManager;
            _resourceRepository = resourceRepository;
            _languageService = languageService;
            _resourceData = resourceData;

            _removalFilters = new List<IRemovalFilter>
            {
                new RemovalFilterContentAll(languageService),
                new RemovalFilterContentPopulated(languageService),
                new RemovalFilterContentMissing()
            };

            _moveResourceOptions = new List<IMoveResource>
            {
                new MoveAllResources(_languageService),
                new MovePopulatedResources(_languageService),
                new MoveMissingResources(),
            };
        }

        public ISearchService Get(SearchOptions searchOptions)
        {
            return new SearchResultsCache(_cacheManager, _resourceRepository, _languageService, searchOptions, this);
        }

        public ISearchService GetBaseSearchService(SearchOptions searchOptions, IResourceRepository resourceRepository)
        {
            return new SearchService(_languageService, searchOptions, this);
        }

        public IRemovalFilter GetRemovalFilter(Content content)
        {
            return _removalFilters.First(x => x.ContentType == content);
        }

        public IMoveResource Get(Content? content)
        {
            return _moveResourceOptions.First(x => x.ContentType == content);
        }

        public IEnumerable<ResourceDataModel> GetResources(SearchOptions searchOptions)
        {
            return !string.IsNullOrWhiteSpace(searchOptions.Key)
                ? _resourceRepository.GetResourcesFilteringByKeyNameAndExclusions(searchOptions.Key).ToList()
                : _resourceRepository.GetResourcesFilteringExclusionOnly().ToList();
        }

        public IFilterChain<SearchResult> GetSearchPipeline(SearchOptions searchOptions)
        {
            return !string.IsNullOrWhiteSpace(searchOptions.Key)
                ? GetSearchPipelineForKeyFilter(searchOptions)
                : GetDefaultSearchPipeline(searchOptions);
        }




        private Pipeline<SearchResult> GetDefaultSearchPipeline(SearchOptions searchOptions)
        {
            var moveResourceOption = Get(searchOptions.Content);
            var removalFilter = GetRemovalFilter(searchOptions.Content);
            var pipeline = new Pipeline<SearchResult>();

            pipeline
                .Register(new RemoveKeysWithNoEnglishKeyAssociation())
                .Register(new TransferDataToOutput())
                .Register(new RemoveResourcesThatCannotBeUsedForSearching(_languageService))
                .Register(new AllContent(_languageService))
                .Register(new PopulatedContent(_languageService))
                .Register(new MissingContent(_languageService))
                .Register(new OnlyFullyPopulated(_languageService))
                .Register(new FilterKeysWithMissingContentAlertDisabled())
                .Register(new TranslationValueFilter())
                .Register(new LanguageFilter(_languageService))
                .Register(new InitialSort())
                .Register(new RemoveResourcesMarkedToBeFiltered(removalFilter)) 
                .Register(new RemoveDuplicateKeys())
                .Register(new FillContextForDefaultLanguage(_resourceData))
                .Register(new ContextFilter(_resourceData))
                .Register(new AddResourceTypes(_resourceData))
                .Register(new ResourceTypeFilter())
                .Register(new SetNonMatchingKeysOfTheSameKey(moveResourceOption))
                .Register(new SortOrder())
                .Register(new SetRootKeys())
                .Register(new FillKeysWithContext());

            return pipeline;
        }

        private Pipeline<SearchResult> GetSearchPipelineForKeyFilter(SearchOptions searchOptions)
        {
            var moveResourceOption = Get(searchOptions.Content);
            var removalFilter = GetRemovalFilter(searchOptions.Content);
            var pipeline = new Pipeline<SearchResult>();

            pipeline
                .Register(new TransferDataToOutput())
                .Register(new AllContent(_languageService))
                .Register(new PopulatedContent(_languageService))
                .Register(new MissingContent(_languageService))
                .Register(new OnlyFullyPopulated(_languageService))
                .Register(new FilterKeysWithMissingContentAlertDisabled())
                .Register(new RemoveResourcesMarkedToBeFiltered(removalFilter))
                .Register(new InitialSort())
                .Register(new SetNonMatchingKeysOfTheSameKey(moveResourceOption))
                .Register(new RemoveDuplicateKeys())
                .Register(new SortOrder())
                .Register(new SetRootKeys())
                .Register(new FillContextForDefaultLanguage(_resourceData))
                .Register(new FillKeysWithContext())
                .Register(new AddResourceTypes(_resourceData));

            return pipeline;
        }
    }
}
