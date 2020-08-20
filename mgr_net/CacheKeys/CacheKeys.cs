namespace mgr_net
{
    public static class CacheKeys
    {
        public static string GetAll { get { return "_All"; } }
        public static string GetById { get { return "_Id"; } }
        public static string GetBySurname { get { return "_Surname"; } }
        public static string GetByTopic { get { return "_Topic"; } }
        public static string NumOfArticles { get { return "_Num"; } }

        public static string FluentCahceRegionKey => "FluentRegion";
    }
}