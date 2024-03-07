using DotNetCourseWebAPI.Data;
using DotNetCourseWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DotNetCourseWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserSalaryEFController : ControllerBase
    {
        private readonly DataContextEF _ef;
        public UserSalaryEFController(IConfiguration config)
        {
            _ef = new DataContextEF(config);
        }

        [HttpGet("GetUserSalary")]
        public IEnumerable<UserSalary> GetUserSalary()
        {
            IEnumerable<UserSalary> userSalaries = _ef.UserSalary.ToList();
            return userSalaries;
        }

        [HttpGet("GetSingleUserSalary/{userId}")]
        public UserSalary GetSingleUserSalary(int userId)
        {
            UserSalary? userSalary = _ef.UserSalary
                .Where(u => u.UserId == userId)
                .FirstOrDefault<UserSalary>();

            if (userSalary != null)
            {
                return userSalary;
            }

            throw new Exception("Failed to get user salary");
        }
    }
}
