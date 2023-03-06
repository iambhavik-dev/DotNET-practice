using rest_api.Models;

namespace rest_api.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public IEnumerable<Users> GetUsers();
        public void AddUsers(Users user);
        public Users GetUserById(string usersId);
        public void UpdateUser(string usersId,Users user);
        public void DeleteUser(string usersId);
    }
}
