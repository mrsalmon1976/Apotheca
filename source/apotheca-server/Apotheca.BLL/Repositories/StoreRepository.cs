using Apotheca.BLL.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.BLL.Repositories
{
    public interface IStoreRepository : IRepository<Store>
    {
        Task<Store> GetById(Guid id);
    }

    public class StoreRepository : Repository<Store>, IStoreRepository
    {
        public StoreRepository(IMongoDatabase mongoDb) : base(mongoDb, "Stores")
        {
        }

        public async Task<Store> GetById(Guid id)
        {
            return await this.Collection.Find<Store>(x => x.Id == id).FirstOrDefaultAsync();
        }

    }
}
