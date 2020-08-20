using System;

namespace mgr_net.DTOs
{
    public class ArticleDto
    {
        public string AuthorName { get; set; }
        public string AuthorSurname { get; set; }
        public string Contents { get; set; }
        public DateTime Created { get; set; }
        public int Length { get; set; }
        public string Summary { get; set; }
        public string Topic { get; set; }
    }
}