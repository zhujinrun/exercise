namespace Crawler.API.Interface
{
    public class MongoDatabaseSettings : IMongoDatabaseSettings
    {

        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string KolPostCollectionName { get; set; }
        public string KolShortCodeCollectionName { get; set; }
    }
}
