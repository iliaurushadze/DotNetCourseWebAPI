using DotNetCourseWebAPI.Data;
using DotNetCourseWebAPI.DTOs;
using DotNetCourseWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCourseWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserEFController : ControllerBase
    {
        private readonly DataContextEF _ef;

        public UserEFController(IConfiguration config)
        {
            _ef = new DataContextEF(config);
        }

        [HttpGet("GetUsers")]
        public IEnumerable<User> GetUsers()
        {
            IEnumerable<User> users = _ef.Users.ToList<User>();
            return users;
        }

        [HttpGet("GetSingleUser/{userId}")]
        public User GetSingleUser(int userId)
        {
            User? user = _ef.Users
                .Where(u => u.UserId == userId)
                .SingleOrDefault<User>();

            if(user != null)
            {
                return user;
            }

            throw new Exception("Failed to get user");
        }

        [HttpPut("EditUser")]
        public IActionResult EditUser(User user)
        {
            User? userDb = _ef.Users
                .Where(u => u.UserId == user.UserId)
                .SingleOrDefault<User>();
            if(userDb != null)
            {
                userDb.FirstName = user.FirstName;
                userDb.LastName = user.LastName;
                userDb.Email = user.Email;
                userDb.Gender = user.Gender;
                userDb.Active = user.Active;
            }

            if (_ef.SaveChanges() > 0)
                return Ok();

            throw new Exception("Failed to update user");
        }

        [HttpPost("AddUser")]
        public IActionResult AddUser(UserToAddDto userToAddDto)
        {
            User userDb = new User();

            userDb.FirstName = userToAddDto.FirstName;
            userDb.LastName = userToAddDto.LastName;
            userDb.Email = userToAddDto.Email;
            userDb.Gender = userToAddDto.Gender;
            userDb.Active = userToAddDto.Active;
            
            _ef.Add(userDb);

            if (_ef.SaveChanges() > 0)
                return Ok();

            throw new Exception("Failed to add user");
        }

        [HttpDelete("DeleteUser/{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            User? userDb = _ef.Users
               .Where(u => u.UserId == userId)
               .SingleOrDefault<User>();

            if (userDb != null)
            {
                _ef.Remove(userDb);
            }

            if (_ef.SaveChanges() > 0)
                return Ok();

            throw new Exception("Failed to delete user");
        }
    }
}
