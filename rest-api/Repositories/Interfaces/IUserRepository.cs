using rest_api.Models;

namespace rest_api.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task<IEnumerable<Users>> GetUsers();
        public Task AddUsers(Users user);
        public Task<Users> GetUserById(string usersId);
        public Task UpdateUser(string usersId, Users user);
        public Task DeleteUser(string usersId);
    }
}
