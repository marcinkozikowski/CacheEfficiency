using System.Collections.Generic;
using System.Threading.Tasks;
using mgr_net.DTOs;
using mgr_net.Entity;

namespace mgr_net.Interfaces
{
    public interface ICacheProxy
    {
        public IEnumerable<ArticleDto> Get();

        public ArticleDto GetById(int id);

        public IEnumerable<ArticleDto> GetByName(string surname);

        public string GetByTopic(string articleTopic);

        public int GetNumOfAuthorArticles(string name, string surname);
    }
}