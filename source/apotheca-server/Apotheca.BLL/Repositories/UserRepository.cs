using Apotheca.BLL.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.BLL.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmail(string email);
    }

    public class UserRepository : IUserRepository
    {
        private IMongoDatabase _mongoDb;
        private IMongoCollection<User> _usersCollection;

        public UserRepository(IMongoDatabase mongoDb)
        {
            _mongoDb = mongoDb;
            _usersCollection = _mongoDb.GetCollection<User>("Users");
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _usersCollection.Find<User>(x => x.Email == email).FirstOrDefaultAsync();
        }
    }
}
