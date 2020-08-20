using System.Collections.Generic;
using System.Linq;
using mgr_net.DTOs;
using mgr_net.Entity;
using mgr_net.Repositories;

namespace mgr_net.CacheStrategies
{
    public class Cacheless : CacheBaseWithLogging
    {
        private readonly IArticleRepository _articleRepository;

        public Cacheless(IArticleRepository articleRepository) : base("Cachless Strategy")
        {
            _articleRepository = articleRepository;
        }
        
        public override IEnumerable<ArticleDto> Get()
        {
            base.Get();
            return _articleRepository.Get().Select(x=>ArticleDtoMapper.MapFrom(x));
        }

        public override ArticleDto GetById(int id)
        {
            base.GetById(id);
            return ArticleDtoMapper.MapFrom(_articleRepository.GetById(id));
        }

        public override IEnumerable<ArticleDto> GetBySurname(string surname)
        {
            base.GetBySurname(surname);
            return _articleRepository.GetBySurname(surname).Select(x=>ArticleDtoMapper.MapFrom(x));
        }

        public override ArticleDto GetByTopic(string articleTopic)
        {
            base.GetByTopic(articleTopic);
            return ArticleDtoMapper.MapFrom(_articleRepository.GetByTopic(articleTopic));
        }

        public override int GetNumOfAuthorArticles(string name, string surname)
        {
            base.GetNumOfAuthorArticles(name, surname);
            return _articleRepository.GetNumOfAuthorArticles(name, surname);
        }
    }
}