using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mgr_net.DTOs;
using mgr_net.Entity;
using mgr_net.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NHibernate;

namespace mgr_net.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArticleController : ControllerBase
    {
        private readonly ISession _session;
        private readonly ICacheProxy _cacheProxy;

        public ArticleController(ISession session,ICacheProxy cacheProxy)
        {
            _session = session;
            _cacheProxy = cacheProxy;
        }

        [HttpGet("/all")]
        public IEnumerable<ArticleDto> Get()
        {
            return _cacheProxy.Get();
        }
        
        [HttpGet("/id/{id}")]
        public ArticleDto GetById(int id)
        {
            return _cacheProxy.GetById(id);
        }
        
        [HttpGet("/article/authorsurname/{surname}")]
        public IEnumerable<ArticleDto> GetBySurname(string surname)
        {
            return _cacheProxy.GetBySurname(surname);
        }
        
        [HttpGet("/topic/author/{articleTopic}")]
        public ArticleDto GetByTopic(string articleTopic)
        {
            return _cacheProxy.GetByTopic(articleTopic);
        }
        
        [HttpGet("/articlesnum/authorname/{name}/authorsurname/{surname}")]
        public int GetNumOfAuthorArticles(string name, string surname)
        {
            return _cacheProxy.GetNumOfAuthorArticles(name, surname);

        }
    }
}