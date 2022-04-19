using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;


//https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mongo-app?view=aspnetcore-6.0&tabs=visual-studio

namespace MongoDemo
{
    public class Book
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        public string BookName { get; set; }

        public decimal Price { get; set; }

        public string Category { get; set; }

        public string Author { get; set; } 
    }
    internal class Program
    {
        public static IMongoCollection<Book> _booksCollection;
        public static void Main(string[] args)
        {
            var settings = MongoClientSettings.FromConnectionString("mongodb://ranahamid:bRaB5pXMvrP7w25n@ranahamid-shard-00-00.pploi.mongodb.net:27017,ranahamid-shard-00-01.pploi.mongodb.net:27017,ranahamid-shard-00-02.pploi.mongodb.net:27017/test?ssl=true&replicaSet=atlas-az72h9-shard-0&authSource=admin&retryWrites=true&w=majority");
            var mongoClient = new MongoClient(settings);
            var mongoDatabase = mongoClient.ListDatabaseNames().ToList();
             
            var database = mongoClient.GetDatabase("test");  
            _booksCollection = database.GetCollection<Book>("entities");

            var newBook = new Book
            {
                Price = 10,
                Author = "TEST",
                BookName = "Book",
                Category = "1",
                //Id = "3"
            };
           CreateAsync(newBook);

           var book =  GetAsync().Result;
        }

        public static async Task<List<Book>> GetAsync() =>
            await _booksCollection.Find(_ => true).ToListAsync();
        public static async Task<Book> GetAsync(string id) =>
            await _booksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public static void CreateAsync(Book newBook) =>
              _booksCollection.InsertOne(newBook);

        public static async Task UpdateAsync(string id, Book updatedBook) =>
            await _booksCollection.ReplaceOneAsync(x => x.Id == id, updatedBook);

        public static async Task RemoveAsync(string id) =>
            await _booksCollection.DeleteOneAsync(x => x.Id == id);
    }
}
