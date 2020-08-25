using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using mgr_net.DTOs;
using mgr_net.Entity;
using mgr_net.Repositories;
using Microsoft.Extensions.Caching.Distributed;

namespace mgr_net.CacheStrategies
{
    public class RedisCacheProxy : CacheBaseWithLogging
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IDistributedCache _distributedCache;

        public RedisCacheProxy(
            IArticleRepository articleRepository, 
            IDistributedCache distributedCache):base("RedisCache Strategy")
        {
            _articleRepository = articleRepository;
            _distributedCache = distributedCache;
        }

        public override IEnumerable<ArticleDto> Get()
        {
            base.Get();
            var serializedValue = _distributedCache.GetString(CacheKeys.GetAll);
            if (string.IsNullOrEmpty(serializedValue))
            {
                var articleEntity = _articleRepository.Get();
                serializedValue = JsonSerializer.Serialize(articleEntity);
                _distributedCache.SetString(CacheKeys.GetAll,serializedValue);
            }
            var resultDto = JsonSerializer.Deserialize<IEnumerable<Article>>(serializedValue).Select(x=>ArticleDtoMapper.MapFrom(x));
            StopWatch();
            return resultDto;
        }

        public override ArticleDto GetById(int id)
        {
            base.GetById(id);
            var serializedValue = _distributedCache.GetString(CacheKeys.GetById + id);
            if (string.IsNullOrEmpty(serializedValue))
            {
                var articleEntity = _articleRepository.GetById(id);
                serializedValue = JsonSerializer.Serialize(articleEntity);
                _distributedCache.SetString(CacheKeys.GetById+id,serializedValue);
            }

            var result = ArticleDtoMapper.MapFrom(JsonSerializer.Deserialize<Article>(serializedValue));
            StopWatch();
            return result;
        }

        public override IEnumerable<ArticleDto> GetByName(string surname)
        {
            base.GetByName(surname);
            var serializedValue = _distributedCache.GetString(CacheKeys.GetBySurname + surname);
            if (string.IsNullOrEmpty(serializedValue))
            {
                var articleEntity = _articleRepository.GetByName(surname);
                serializedValue = JsonSerializer.Serialize(articleEntity);
                _distributedCache.SetString(CacheKeys.GetBySurname+surname,serializedValue);
            }
            var result =  JsonSerializer.Deserialize<IEnumerable<Article>>(serializedValue).Select(x=>ArticleDtoMapper.MapFrom(x));
            StopWatch();
            return result;
        }

        public override string GetByTopic(string articleTopic)
        {
            base.GetByTopic(articleTopic);
            var serializedValue = _distributedCache.GetString(CacheKeys.GetByTopic + articleTopic);
            if (string.IsNullOrEmpty(serializedValue))
            {
                var articleEntity = _articleRepository.GetByTopic(articleTopic);
                serializedValue = articleEntity;
                _distributedCache.SetString(CacheKeys.GetByTopic+articleTopic,articleEntity);
            }
            var result = serializedValue;
            StopWatch();
            return result;
        }

        public override int GetNumOfAuthorArticles(string name, string surname)
        {
            base.GetNumOfAuthorArticles(name,surname);
            var serializedValue = _distributedCache.GetString(CacheKeys.NumOfArticles + name+surname);
            if (string.IsNullOrEmpty(serializedValue))
            {
                var articleEntity = _articleRepository.GetNumOfAuthorArticles(name,surname);
                serializedValue = articleEntity.ToString();
                _distributedCache.SetString(CacheKeys.NumOfArticles+name+surname,serializedValue);
            }
            StopWatch();
            return int.Parse(serializedValue);
        }
    }
}