using System;
using System.Collections.Generic;
using System.Linq;
using LazyCache;
using mgr_net.DTOs;
using mgr_net.Repositories;

namespace mgr_net.CacheStrategies
{
    public class LazyCacheProxy : CacheBaseWithLogging
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IAppCache _appCahe;

        public LazyCacheProxy(IArticleRepository articleRepository, IAppCache appCahe) : base("LazyCache Strategy")
        {
            _articleRepository = articleRepository;
            _appCahe = appCahe;
        }

        public override IEnumerable<ArticleDto> Get()
        {
            base.Get();
            var result = GetOrAddToCache(() => _articleRepository.Get(), CacheKeys.GetAll);
            return result.Select(x => ArticleDtoMapper.MapFrom(x));
        }

        public override ArticleDto GetById(int id)
        {
            base.GetById(id);
            var result = GetOrAddToCache(() => _articleRepository.GetById(id), CacheKeys.GetById + id);
            
            return ArticleDtoMapper.MapFrom(result);
        }

        public override IEnumerable<ArticleDto> GetBySurname(string surname)
        {
            base.GetBySurname(surname);
            var result = GetOrAddToCache(() => _articleRepository.GetBySurname(surname), CacheKeys.GetBySurname + surname);
            
            return result.Select(x => ArticleDtoMapper.MapFrom(x));
        }

        public override ArticleDto GetByTopic(string articleTopic)
        {
            base.GetByTopic(articleTopic);
            var result = GetOrAddToCache(() => _articleRepository.GetByTopic(articleTopic), CacheKeys.GetByTopic + articleTopic);

            return ArticleDtoMapper.MapFrom(result);
        }

        public override int GetNumOfAuthorArticles(string name, string surname)
        {
            base.GetNumOfAuthorArticles(name, surname);
            var result = GetOrAddToCache(() => _articleRepository.GetNumOfAuthorArticles(name,surname), CacheKeys.NumOfArticles + name+surname);

            return result;
        }
        
        private T GetOrAddToCache<T>(Func<T> dbMethod, string key)
        {
            T value = _appCahe.Get<T>(key);
            if (value == null)
            {
                value = dbMethod();
                _appCahe.Add(key,value);
            }

            return value;
        }
    }
}