using System.Collections.Generic;
using mgr_net.DTOs;
using mgr_net.Entity;

namespace mgr_net.Interfaces
{
    public interface ICacheProxy
    {
        public IEnumerable<ArticleDto> Get();

        public ArticleDto GetById(int id);

        public IEnumerable<ArticleDto> GetBySurname(string surname);

        public ArticleDto GetByTopic(string articleTopic);

        public int GetNumOfAuthorArticles(string name, string surname);
    }
}