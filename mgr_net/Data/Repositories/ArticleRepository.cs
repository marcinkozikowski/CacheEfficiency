using System.Collections.Generic;
using System.Linq;
using mgr_net.Entity;
using NHibernate;

namespace mgr_net.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly ISession _session;

        public ArticleRepository(ISession session)
        {
            _session = session;
        }

        public IEnumerable<Article> Get()
        {
            return _session.Query<Article>().ToList();
        }

        public Article GetById(int id)
        {
            return _session.Get<Article>(id);
        }

        public IEnumerable<Article> GetBySurname(string surname)
        {
            return _session.Query<Article>().Where(x => x.AuthorSurname == surname)
                .ToList();
        }

        public Article GetByTopic(string articleTopic)
        {
            return _session.Query<Article>().FirstOrDefault(x => x.Topic == articleTopic);
        }

        public int GetNumOfAuthorArticles(string name, string surname)
        {
            return _session.Query<Article>().Count(x => x.AuthorName == name && x.AuthorSurname == surname);
        }
    }
}