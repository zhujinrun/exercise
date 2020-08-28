namespace Crawler.API.Interface
{
    public interface IMongoDatabaseSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string KolPostCollectionName { get; set; }
        string KolShortCodeCollectionName { get; set; }
    }
}
