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

        Task<IEnumerable<Store>> GetByIds(IEnumerable<Guid> ids);
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

        public async Task<IEnumerable<Store>> GetByIds(IEnumerable<Guid> ids)
        {
            var filter = Builders<Store>.Filter.In(x => x.Id, ids);
            return await this.Collection.Find<Store>(filter).ToListAsync();
        }

    }
}
