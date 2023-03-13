using Amazon.Util.Internal.PlatformServices;
using MongoDB.Bson;
using MongoDB.Driver;
using rest_api.Models;
using rest_api.Repositories.Interfaces;

namespace rest_api.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly string DatabaseName = "REST";
        private readonly string CollectionName = "Users";

        private readonly IMongoCollection<Users> _usersCollection;
        private readonly FilterDefinitionBuilder<Users> _filterDefinitionBuilder = Builders<Users>.Filter;
        public UserRepository(IMongoClient _mongoClient)
        {
            IMongoDatabase db = _mongoClient.GetDatabase(DatabaseName);
            _usersCollection = db.GetCollection<Users>(CollectionName);
        }
        public async Task<IEnumerable<Users>> GetUsers()
        {
            var data = await _usersCollection.Find(new BsonDocument()).ToListAsync();
            return data;
        }

        public async Task AddUsers(Users user)
        {
            await _usersCollection.InsertOneAsync(user);
        }

        public async Task<Users> GetUserById(string usersId)
        {
            return await _usersCollection
                .Find(user => user.Id.Equals(usersId))
                .SingleOrDefaultAsync();
        }

        public async Task UpdateUser(string usersId, Users user)
        {
            await _usersCollection.ReplaceOneAsync(user => user.Id.Equals(usersId), user);
        }

        public async Task DeleteUser(string usersId)
        {
            await _usersCollection.DeleteOneAsync(user => user.Id.Equals(usersId));
        }
    }
}
