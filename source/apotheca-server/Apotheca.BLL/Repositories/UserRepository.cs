using Apotheca.BLL.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.BLL.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmail(string email);
    }

    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(IMongoDatabase mongoDb) : base(mongoDb, "Users")
        {
        }

        public async Task<User> GetByEmail(string email)
        {
            return await this.Collection.Find<User>(x => x.Email == email).FirstOrDefaultAsync();
        }

    }
}
