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
        public IEnumerable<Users> GetUsers()
        {
            var data = _usersCollection.Find(new BsonDocument()).ToList();
            return data;
        }

        public void AddUsers(Users user)
        {
            _usersCollection.InsertOne(user);
        }

        public Users GetUserById(string usersId)
        {
            return _usersCollection
                .Find(user => user.Id.Equals(usersId))
                .SingleOrDefault();
        }

        public void UpdateUser(string usersId, Users user)
        {
            _usersCollection.ReplaceOne(user => user.Id.Equals(usersId), user);
        }

        public void DeleteUser(string usersId)
        {
            _usersCollection.DeleteOne(user => user.Id.Equals(usersId));
        }
    }
}
