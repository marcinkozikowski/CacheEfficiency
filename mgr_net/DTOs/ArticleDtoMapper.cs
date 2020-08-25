using System;
using mgr_net.Entity;

namespace mgr_net.DTOs
{
    public static class ArticleDtoMapper
    {
        /// <summary>
        /// Maps Article entity to Article data transfer object
        /// </summary>
        /// <param name="input">Article entity</param>
        /// <returns>Article DTO</returns>
        public static ArticleDto MapFrom(Article input)
        {
            if (input == null)
            {
                return new ArticleDto();
            }
            
            return new ArticleDto()
            {
                Contents = input?.Contents,
                Length = input.Length,
                Created = input.Created,
                Summary = input?.Summary,
                Topic = input?.Topic,
                AuthorName = input?.AuthorName,
                AuthorSurname = input?.AuthorSurname,
            };
        }
    }
}