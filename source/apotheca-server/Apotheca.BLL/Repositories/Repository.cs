using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Apotheca.BLL.Repositories
{
    public interface IRepository<T> where T : class
    {
        void Insert(T document);
    }
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        public Repository(IMongoDatabase mongoDb, string collectionName)
        {
            this.MongoDb = mongoDb;
            this.Collection = this.MongoDb.GetCollection<T>(collectionName);
        }

        protected IMongoDatabase MongoDb { get; set; }

        protected IMongoCollection<T> Collection { get; set; }

        public void Insert(T document)
        {
            this.Collection.InsertOne(document);
        }
    }
}
