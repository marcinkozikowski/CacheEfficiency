using System;
using System.Collections.Generic;
using FluentCache;
using mgr_net.DTOs;
using mgr_net.Entity;
using mgr_net.Interfaces;
using Microsoft.Extensions.Logging;

namespace mgr_net.CacheStrategies
{
    public abstract class CacheBaseWithLogging : ICacheProxy
    {
        private string CacheStrategyName { get;}

        private bool IsLogginEneable { get; set; }

        protected CacheBaseWithLogging(string cacheStrategyName)
        {
            CacheStrategyName = cacheStrategyName;
            IsLogginEneable = false;
        }

        public virtual IEnumerable<ArticleDto> Get()
        {
            LogInfo($"Get All records by {CacheStrategyName}");
            return null;
        }

        public virtual ArticleDto GetById(int id)
        {
            LogInfo($"Get by article id: {id} by {CacheStrategyName}");
            return null;
        }

        public virtual IEnumerable<ArticleDto> GetBySurname(string surname)
        {
            LogInfo($"Get Article by Author surname {surname} by {CacheStrategyName}");
            return null;
        }

        public virtual ArticleDto GetByTopic(string articleTopic)
        {
            LogInfo($"Get articles by topic: {articleTopic} by {CacheStrategyName}");
            return null;
        }

        public virtual int GetNumOfAuthorArticles(string name, string surname)
        {
            LogInfo($"Get number of articles by {CacheStrategyName}");
            return 0;
        }

        private void LogInfo(string logInformation)
        {
            if (IsLogginEneable)
            {
                Console.WriteLine(logInformation);
            }
        }
    }
}