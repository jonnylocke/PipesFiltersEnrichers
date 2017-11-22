using System;
using System.Collections.Generic;
using System.Linq;
using Iglu.PlanetCruise.Translations.Caching.Interfaces;
using Iglu.PlanetCruise.Translations.DAL.Models;
using Iglu.PlanetCruise.Translations.Models.DomainModels;
using Iglu.PlanetCruise.Translations.Models.Dtos;

namespace Iglu.PlanetCruise.Translations.BusinessLogic.Services.Impl.Search.Filters
{
    internal class ContextFilter : FilterBase<SearchResult>
    {
        private const int DefaultLanguageId = 1;
        private readonly IEnumerable<ResourceContextDataModel> _resourceContexts;

        public ContextFilter(IResourceData resourceData)
        {
            _resourceContexts = resourceData.GetAllResourceContexts();
        }

        protected override SearchResult Process(SearchResult value)
        {
            if (!IsApplicable(value)) return value;

            base.SetInboundKeysTotal(value);
            base.ProcessStartTime = DateTime.Now;

            RemoveKeysWithNoContext(value);
            RemoveKeysNotLikeTheContext(value);

            base.StopProcessTime(value, this.GetType().Name);

            return value;
        }

        protected override bool IsApplicable(SearchResult value)
        {
            return !string.IsNullOrWhiteSpace(value.SearchOptions.Context);
        }


        

        

        private void RemoveKeysWithNoContext(SearchResult value)
        {
            var defaultLangauageKeys = value.Output
                .GroupBy(x => x.LanguageId == DefaultLanguageId)
                .Where(x => x.Key)
                .SelectMany(grp => grp)
                .ToList();

            var keysToRemove = defaultLangauageKeys
                .Where(x => _resourceContexts.All(y => y.Name.ToLower() != x.Name.ToLower())).ToList();

            base.RemoveKeysFromOutput(value, keysToRemove);
        }

        private void RemoveKeysNotLikeTheContext(SearchResult value)
        {
            var englishKeys = value.Data
                .GroupBy(x => x.LanguageId == DefaultLanguageId)
                .Where(x => x.Key)
                .SelectMany(grp => grp)
                .ToList();

            var notMatchingKeys = englishKeys.GroupBy(
                x => !x.Context.ToLower().Contains(value.SearchOptions.Context.ToLower()))
                .Where(x => x.Key).SelectMany(grp => grp).ToList();

            var keysToRemove = new List<ResourceComposite>();

            foreach (var key in notMatchingKeys)
                keysToRemove.AddRange(
                    value.Output.Where(x => 
                    string.Equals(x.Name, key.Name, StringComparison.CurrentCultureIgnoreCase)));

            base.RemoveKeysFromOutput(value, keysToRemove);
        }
    }
}
