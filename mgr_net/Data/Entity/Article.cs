using System;

namespace mgr_net.Entity
{
    public class Article
    {
        public virtual int Id { get; set; }
        public virtual bool Active { get; set; }
        public virtual string AuthorName { get; set; }
        public virtual string AuthorSurname { get; set; }
        public virtual string Contents { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual int Length { get; set; }
        public virtual string Summary { get; set; }
        public virtual string Topic { get; set; }
    }
}