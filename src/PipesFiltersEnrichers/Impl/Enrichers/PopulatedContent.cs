using System;
using System.Collections.Generic;
using System.Linq;
using Iglu.PlanetCruise.Translations.BusinessLogic.Services.Impl.Search.Filters;
using Iglu.PlanetCruise.Translations.Models.DomainModels;
using Iglu.PlanetCruise.Translations.Models.Dtos;
using Iglu.PlanetCruise.Translations.Models.Enums;
using Iglu.PlanetCruise.Translations.Services;

namespace Iglu.PlanetCruise.Translations.BusinessLogic.Services.Impl.Search.Enrichers
{
    internal class PopulatedContent : FilterBase<SearchResult>
    {
        private readonly IEnumerable<LanguageDTO> _allLanguagesFromSvc;

        public PopulatedContent(ILanguageService languageService)
        {
            _allLanguagesFromSvc = languageService.GetAllLanguages();
        }

        protected override SearchResult Process(SearchResult value)
        {
            if (!IsApplicable(value)) return value;

            base.SetInboundKeysTotal(value);
            base.ProcessStartTime = DateTime.Now;
            AddMissingTranslationKeys(value);
            base.StopProcessTime(value, this.GetType().Name);

            return value;
        }

        protected override bool IsApplicable(SearchResult value)
        {
            return value.SearchOptions.Content == Content.Populated && !string.IsNullOrWhiteSpace(value.SearchOptions.Language);
        }





        private void AddMissingTranslationKeys(SearchResult value)
        {
            var keysWithMissingTranslations = GetKeysWithMissingTranslations(value.Data);
            var result = value.Data.ToList();
            
            result.AddRange(keysWithMissingTranslations);

            value.Data = result;
        }

        private IEnumerable<ResourceComposite> GetKeysWithMissingTranslations(IEnumerable<ResourceComposite> allResources)
        {
            var keyGroups = allResources.GroupBy(x => x.Name);

            var missingResources = new List<ResourceComposite>();

            foreach (var keyGroup in keyGroups)
            {
                var keys = keyGroup.ToList();
                var missingLanguages = _allLanguagesFromSvc.Where(x => keys.All(x2 => x2.LanguageId != x.Id)).ToList();

                if (!missingLanguages.Any()) continue;
                {
                    var englishKey = keys.FirstOrDefault(x => x.LanguageId == 1);
                    var keyToUse = englishKey ?? keys.First();
                    missingResources.AddRange(missingLanguages.Select(missingLanguage => CreateNewEmptyMissingResource(keyToUse, missingLanguage)));
                }
            }

            return missingResources;
        }


        private static ResourceComposite CreateNewEmptyMissingResource(ResourceComposite item, LanguageDTO lang)
        {
            return new ResourceComposite
            {
                LanguageId = lang.Id,
                Language = lang,
                Name = item.Name,
                Context = item.Context,
                Section = item.Section,
                TypeId = item.TypeId,
                WebSiteId = item.WebSiteId,
                CreatedDate = item.CreatedDate,
                UpdatedDate = item.UpdatedDate,
                IsMissing = true,
                ToBeFiltered = true,
                MissingContentAlertDisabled = item.MissingContentAlertDisabled
            };
        }
    }
}
