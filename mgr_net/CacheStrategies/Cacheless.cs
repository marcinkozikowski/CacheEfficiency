using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var result =  ArticleDtoMapper.MapFrom(_articleRepository.GetById(id));
            StopWatch();
            return result;
        }

        public override IEnumerable<ArticleDto> GetByName(string surname)
        {
            base.GetByName(surname);
            var result = _articleRepository.GetByName(surname).Select(x=>ArticleDtoMapper.MapFrom(x));
            StopWatch();
            return result;
        }

        public override string GetByTopic(string articleTopic)
        {
            base.GetByTopic(articleTopic);
            var result = _articleRepository.GetByTopic(articleTopic);
            StopWatch();
            return result;
        }

        public override int GetNumOfAuthorArticles(string name, string surname)
        {
            base.GetNumOfAuthorArticles(name, surname);
            var result = _articleRepository.GetNumOfAuthorArticles(name, surname);
            StopWatch();
            return result;
        }
    }
}