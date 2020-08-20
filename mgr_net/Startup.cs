using System;
using System.Linq;
using FluentCache.Simple;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using LazyCache;
using mgr_net.CacheStrategies;
using mgr_net.Interfaces;
using mgr_net.Mappings;
using mgr_net.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace mgr_net
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var cacheStrategy = Configuration.GetValue<string>("cache");

            
            services.AddControllers();
            services.AddScoped<IArticleRepository, ArticleRepository>();
            
            if (cacheStrategy == "fluent")
            {
                AddFluentCache(services);
                Console.WriteLine("Running with FluentCache Strategy");
            }
            else if (cacheStrategy == "lazy")
            {
                Console.WriteLine("Running with LazyCache Strategy");
                services.AddScoped<ICacheProxy, LazyCacheProxy>();
                services.AddSingleton<IAppCache, CachingService>();
            }
            else if (cacheStrategy == "memory")
            {
                AddMemoryCache(services);
                Console.WriteLine("Running with MemoryCache Strategy");
            }
            else if (cacheStrategy == "redis")
            {
                Console.WriteLine("Running with RedisCache Strategy");
                services.AddScoped<ICacheProxy, RedisCacheProxy>();
                services.AddDistributedRedisCache(option =>
                {
                    option.Configuration = "127.0.0.1";
                    option.InstanceName = "master";
                });
            }
            else
            {
                services.AddScoped<ICacheProxy, Cacheless>();
                Console.WriteLine("Running with Cachless Strategy");
            }
            

            services.AddSingleton<NHibernate.ISessionFactory>(factory =>
            {
                return Fluently.Configure()
                    .Database(PostgreSQLConfiguration.Standard
                        .ConnectionString(x=>x.Database("mgr").Host("localhost").Password("admin").Port(5432).Username("postgres")))
                    .Mappings(m =>m.FluentMappings.AddFromAssemblyOf<ArticleNhMapping>())
                    .CurrentSessionContext("call")
                    .BuildSessionFactory();
            }); 

            services.AddSingleton<NHibernate.ISession>(factory =>
                factory
                    .GetServices<NHibernate.ISessionFactory>()
                    .First()
                    .OpenSession()
            );
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Praca Magisterska API",
                    Description = "Aplikacja wykonana na potrzeby pracy magisterskiej w celu testowania wydajności mechanizmów cache na platformie .NET Core",
                    Contact = new OpenApiContact
                    {
                        Name = "Marcin Kozikowski",
                        Email = "marcinkozikowski@wp.pl",
                        Url = new Uri("https://github.com/marcinkozikowski"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under GNU",
                        Url = new Uri("https://www.gnu.org/licenses/gpl-3.0.html"),
                    }
                });
            });
        }

        private static void AddFluentCache(IServiceCollection services)
        {
            services.AddSingleton<FluentCache.ICache, FluentDictionaryCache>();
            services.AddScoped<ICacheProxy, CacheStrategies.FluentCacheProxy>();
        }

        private static void AddMemoryCache(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddScoped<ICacheProxy, CacheStrategies.MemoryCacheProxy>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Praca Magisterska .NET Core API V1");
            });
        }
    }
}