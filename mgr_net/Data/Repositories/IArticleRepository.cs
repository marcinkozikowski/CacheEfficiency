using System.Collections.Generic;
using System.Threading.Tasks;
using mgr_net.Entity;

namespace mgr_net.Repositories
{
    public interface IArticleRepository
    {
        /// <summary>
        /// Gets all records from Article table
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Article> Get();

        /// <summary>
        /// Gets Article by it`s Id
        /// </summary>
        /// <param name="id">Article identyfier</param>
        /// <returns>Article</returns>
        public Article GetById(int id);

        /// <summary>
        /// Gets all articles wrtitten by autohr surname
        /// </summary>
        /// <param name="surname">Autoh surname</param>
        /// <returns>List of Articles</returns>
        public IEnumerable<Article> GetByName(string surname);

        /// <summary>
        /// Gets Article by it`s topic
        /// </summary>
        /// <param name="articleTopic">Article topic</param>
        /// <returns>Article</returns>
        public string GetByTopic(string articleTopic);

        /// <summary>
        /// Gets number of articles written by specyfied author
        /// </summary>
        /// <param name="name">Athor name</param>
        /// <param name="surname">Author surname</param>
        /// <returns>Number of articles written by specyfied author</returns>
        public int GetNumOfAuthorArticles(string name, string surname);
    }
}