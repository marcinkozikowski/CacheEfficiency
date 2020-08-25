using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly ICacheProxy _cacheProxy;

        public ArticleController(ICacheProxy cacheProxy)
        {
            _cacheProxy = cacheProxy;
        }

        [HttpGet("/rest/article/all")]
        public IEnumerable<ArticleDto> Get()
        {
            return _cacheProxy.Get();
        }
        
        [HttpGet("/rest/article/id/{id}")]
        public ArticleDto GetById(int id)
        {
            var result = _cacheProxy.GetById(id);
            return result;

        }
        
        [HttpGet("/rest/article/name/{surname}")]
        public IEnumerable<ArticleDto> GetBySurname(string surname)
        {
            return _cacheProxy.GetByName(surname);
        }
        
        [HttpGet("/rest/topic/author/{articleTopic}")]
        public string GetByTopic(string articleTopic)
        {
            return _cacheProxy.GetByTopic(articleTopic);
        }
        
        [HttpGet("/rest/articlesnum/authorname/{name}/authorsurname/{surname}")]
        public int GetNumOfAuthorArticles(string name, string surname)
        {
            return _cacheProxy.GetNumOfAuthorArticles(name, surname);

        }
    }
}