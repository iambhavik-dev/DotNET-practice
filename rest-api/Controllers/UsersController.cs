using Microsoft.AspNetCore.Mvc;
using rest_api.Models;
using rest_api.Repositories.Interfaces;

namespace rest_api.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public UsersController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpGet]
        public ActionResult<Users> GetUsers()
        {
            var result = userRepository.GetUsers();
            return Ok(result);
        }

        [HttpPost]
        public ActionResult<Users> AddUsers(Users user)
        {
            userRepository.AddUsers(user);
            return Ok();
        }

        [HttpGet]
        [Route("{userId}")]
        public ActionResult<Users> GetUsers(string userId)
        {
            var result = userRepository.GetUserById(userId);
            return Ok(result);
        }

        [HttpPost]
        [Route("{userId}")]
        public ActionResult<Users> UpdateUser(string userId, Users user)
        {
            userRepository.UpdateUser(userId, user);
            return Ok();
        }

        [HttpDelete]
        [Route("{userId}")]
        public ActionResult<Users> DeleteUser(string userId)
        {
            userRepository.DeleteUser(userId);
            return Ok();
        }

    }
}
