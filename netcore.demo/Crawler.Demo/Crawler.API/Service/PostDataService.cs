using Crawler.API.Interface;
using Crawler.API.Model;
using MongoDB.Driver;
using System.Collections.Generic;

namespace Crawler.API.Services
{
    public class PostDataService
    {
        private readonly IMongoCollection<Data> _postDataService;

        public PostDataService(IMongoDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);   //数据库链接 
            var databse = client.GetDatabase(settings.DatabaseName);    //数据库名
            _postDataService = databse.GetCollection<Data>(settings.KolPostCollectionName);   //
        }
       
        public List<Data> Get() => _postDataService.Find(Data => true).ToList();
        public Data Get(string id) => _postDataService.Find<Data>(t => t.Id == id).FirstOrDefault();

        public Data Create(Data data)
        {
            _postDataService.InsertOne(data);
            return data;
        }
        public void Update(string id, Data data) => _postDataService.ReplaceOne(pd => pd.Id == id, data);

        public void Remove(Data dataDel) => _postDataService.DeleteOne(t => t.Id == dataDel.Id);

        public void Remove(string id) => _postDataService.DeleteOne(t => t.Id == id);
    }
}
