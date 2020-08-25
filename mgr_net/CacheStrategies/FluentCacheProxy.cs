using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentCache;
using mgr_net.DTOs;
using mgr_net.Repositories;

namespace mgr_net.CacheStrategies
{
    public class FluentCacheProxy : CacheBaseWithLogging
    {
        private readonly ICache _fluentCache;
        private readonly IArticleRepository _articleRepository;

        public FluentCacheProxy(
            ICache fluentCache,
            IArticleRepository articleRepository) : base("FluentCache Strategy")
        {
            _fluentCache = fluentCache;
            _articleRepository = articleRepository;
        }

        public override IEnumerable<ArticleDto> Get()
        {
            base.Get();
            var result = GetOrAddToCache(() => _articleRepository.Get(), CacheKeys.GetAll);
            var resultDto = result.Select(x => ArticleDtoMapper.MapFrom(x));
            StopWatch();
            return resultDto;
        }

        public override ArticleDto GetById(int id)
        {
            base.GetById(id);
            var result = GetOrAddToCache(() => _articleRepository.GetById(id), CacheKeys.GetById + id);
            
            var resultDto = ArticleDtoMapper.MapFrom(result);
            StopWatch();
            return resultDto;
        }

        public override IEnumerable<ArticleDto> GetByName(string surname)
        {
            base.GetByName(surname);
            var result = GetOrAddToCache(() => _articleRepository.GetByName(surname), CacheKeys.GetBySurname + surname);
            var resultDto = result.Select(x => ArticleDtoMapper.MapFrom(x));
            StopWatch();
            return resultDto;
        }

        public override string GetByTopic(string articleTopic)
        {
            base.GetByTopic(articleTopic);
            var result = GetOrAddToCache(() => _articleRepository.GetByTopic(articleTopic), CacheKeys.GetByTopic + articleTopic);
            
            var resultDto = result;
            StopWatch();
            return resultDto;
        }

        public override int GetNumOfAuthorArticles(string name, string surname)
        {
            base.GetNumOfAuthorArticles(name, surname);
            var result = GetOrAddToCache(() => _articleRepository.GetNumOfAuthorArticles(name,surname), CacheKeys.NumOfArticles + name+surname);
            
            StopWatch();
            return result;
        }
        
        private T GetOrAddToCache<T>(Func<T> dbMethod, string key)
        {
            T cacheEntry = default(T);
            try
            {
                cacheEntry = _fluentCache.Get<T>(key, CacheKeys.FluentCahceRegionKey)
                    .Value;
            }
            catch
            {
                if (cacheEntry == null || cacheEntry.Equals(default(T)))
                {
                    // Key not in cache, so get data.
                    cacheEntry = dbMethod();
                    if (cacheEntry != null)
                    {
                        _fluentCache.Set(key, CacheKeys.FluentCahceRegionKey, cacheEntry,
                            new CacheExpiration());
                    }
                }
            }

            return cacheEntry;
        }
    }
}