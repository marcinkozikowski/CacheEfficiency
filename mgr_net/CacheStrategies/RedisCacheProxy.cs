using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
            return JsonSerializer.Deserialize<IEnumerable<Article>>(serializedValue).Select(x=>ArticleDtoMapper.MapFrom(x));
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
            return ArticleDtoMapper.MapFrom(JsonSerializer.Deserialize<Article>(serializedValue));
        }

        public override IEnumerable<ArticleDto> GetBySurname(string surname)
        {
            base.GetBySurname(surname);
            var serializedValue = _distributedCache.GetString(CacheKeys.GetBySurname + surname);
            if (string.IsNullOrEmpty(serializedValue))
            {
                var articleEntity = _articleRepository.GetBySurname(surname);
                serializedValue = JsonSerializer.Serialize(articleEntity);
                _distributedCache.SetString(CacheKeys.GetBySurname+surname,serializedValue);
            }
            return JsonSerializer.Deserialize<IEnumerable<Article>>(serializedValue).Select(x=>ArticleDtoMapper.MapFrom(x));
        }

        public override ArticleDto GetByTopic(string articleTopic)
        {
            base.GetByTopic(articleTopic);
            var serializedValue = _distributedCache.GetString(CacheKeys.GetByTopic + articleTopic);
            if (string.IsNullOrEmpty(serializedValue))
            {
                var articleEntity = _articleRepository.GetByTopic(articleTopic);
                serializedValue = JsonSerializer.Serialize(articleEntity);
                _distributedCache.SetString(CacheKeys.GetByTopic+articleTopic,serializedValue);
            }
            return ArticleDtoMapper.MapFrom(JsonSerializer.Deserialize<Article>(serializedValue));
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

            return int.Parse(serializedValue);
        }
    }
}