
using FluentNHibernate.Mapping;
using mgr_net.Entity;

namespace mgr_net.Mappings
{
    public class ArticleNhMapping : ClassMap<Article>
    {
        public ArticleNhMapping()
        {
            Table("article");
            
            Schema("public");

            Id(x => x.Id).Column("id");
            Map(x => x.Active).Column("active");
            Map(x => x.Contents).Column("contents");
            Map(x => x.Created).Column("created");
            Map(x => x.Length).Column("length");
            Map(x => x.Summary).Column("summary");
            Map(x => x.Topic).Column("topic");
            Map(x => x.AuthorName).Column("author_name");
            Map(x => x.AuthorSurname).Column("author_surname");
        }
    }
}