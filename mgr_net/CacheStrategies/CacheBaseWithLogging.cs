using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
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
        private Stopwatch watch { get; set; }

        private bool IsLogginEneable { get; set; }

        protected CacheBaseWithLogging(string cacheStrategyName)
        {
            CacheStrategyName = cacheStrategyName;
            IsLogginEneable = true;
            watch = new Stopwatch();
        }

        public virtual IEnumerable<ArticleDto> Get()
        {
            LogInfo();
            //LogInfo($"Get All records by {CacheStrategyName}");
            return null;
        }

        public virtual ArticleDto GetById(int id)
        {
            //LogInfo($"Get by article id: {id} by {CacheStrategyName}");
            LogInfo();
            return null;
        }

        public virtual IEnumerable<ArticleDto> GetByName(string surname)
        {
            LogInfo();
            //LogInfo($"Get Article by Author surname {surname} by {CacheStrategyName}");
            return null;
        }

        public virtual string GetByTopic(string articleTopic)
        {
            LogInfo();
            //LogInfo($"Get articles by topic: {articleTopic} by {CacheStrategyName}");
            return null;
        }

        public virtual int GetNumOfAuthorArticles(string name, string surname)
        {
            LogInfo();
            //LogInfo($"Get number of articles by {CacheStrategyName}");
            return 0;
        }

        public void StopWatch()
        {
            if (watch != null && IsLogginEneable)
            {
                watch.Stop();
                Console.WriteLine(DateTimeOffset.Now.ToUnixTimeMilliseconds()+"\t"+(watch.ElapsedTicks * 1000000000 / Stopwatch.Frequency));
            }
        }

        private void LogInfo(string logInformation="")
        {
            if (IsLogginEneable)
            {
                watch.Start();
                Console.Write(logInformation);
            }
        }
    }
}