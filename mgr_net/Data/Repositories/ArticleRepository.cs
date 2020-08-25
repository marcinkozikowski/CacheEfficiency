using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mgr_net.Entity;
using NHibernate;
using NHibernate.Linq;

namespace mgr_net.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly ISession _session;

        public ArticleRepository(ISession session)
        {
            _session = session;
            //_session.CacheMode = CacheMode.Ignore;
        }

        public IEnumerable<Article> Get()
        {
            return  _session.Query<Article>().ToList();
        }

        public Article GetById(int id)
        {
            return _session.Get<Article>(id);
        }

        public IEnumerable<Article> GetByName(string surname)
        {
            return _session.Query<Article>().Where(x => x.AuthorName == surname).OrderBy(x => x.Id)
                .Take(20).ToList();
        }

        public string GetByTopic(string articleTopic)
        {
            return _session.Query<Article>().Where(x => x.Topic == articleTopic).Select(x=>x.AuthorName +x.AuthorSurname).FirstOrDefault();
        }

        public int GetNumOfAuthorArticles(string name, string surname)
        {
            return _session.Query<Article>().Where(x => x.AuthorName == name && x.AuthorSurname == surname).Count();
        }
    }
}