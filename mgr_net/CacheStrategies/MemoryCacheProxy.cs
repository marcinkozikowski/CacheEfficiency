using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using mgr_net.DTOs;
using mgr_net.Entity;
using mgr_net.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace mgr_net.CacheStrategies
{
    public class MemoryCacheProxy : CacheBaseWithLogging
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IArticleRepository _articleRepository;

        public MemoryCacheProxy(IArticleRepository articleRepository, IMemoryCache memoryCache) : base(
            "MemoryCache Strategy")
        {
            _articleRepository = articleRepository;
            _memoryCache = memoryCache;
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
            var resultDto= result.Select(x => ArticleDtoMapper.MapFrom(x));
            StopWatch();
            return resultDto;
        }

        public override string GetByTopic(string articleTopic)
        {
            base.GetByTopic(articleTopic);
            var result = GetOrAddToCache(() => _articleRepository.GetByTopic(articleTopic), CacheKeys.GetByTopic + articleTopic);
            var reslutDto = result;
            StopWatch();
            return reslutDto;
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
            T cacheEntry;

            // Look for cache key.
            if (!_memoryCache.TryGetValue(key, out cacheEntry))
            {
                // Key not in cache, so get data.
                //Console.WriteLine("Going to DB for data");
                cacheEntry = dbMethod();

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(10));

                if (cacheEntry != null)
                {
                    // Save data in cache.
                    _memoryCache.Set(key, cacheEntry, cacheEntryOptions);
                }
            }

            return cacheEntry;
        }
    }
}