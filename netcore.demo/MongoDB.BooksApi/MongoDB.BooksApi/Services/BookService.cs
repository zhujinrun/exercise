using MongoDB.BooksApi.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDB.BooksApi.Services
{
    public class BookService
    {
        private readonly IMongoCollection<Book> _books;
        public BookService(IBookstoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);   //数据库链接 
            var databse = client.GetDatabase(settings.DatabaseName);    //表名
            _books = databse.GetCollection<Book>(settings.BooksCollectionName);   //数据库名
        }

        public List<Book> Get() => _books.Find(book => true).ToList();
        public Book Get(string id) => _books.Find<Book>(book => book.Id== id).FirstOrDefault();

        public Book Create(Book book)
        {
            _books.InsertOne(book);
            return book;
        }
        public void Update(string id, Book book) => _books.ReplaceOne(book => book.Id == id, book);

        public void Remove(Book bookDel) => _books.DeleteOne(book => book.Id == bookDel.Id);

        public void Remove(string id) => _books.DeleteOne(book => book.Id== id);
    }
}
