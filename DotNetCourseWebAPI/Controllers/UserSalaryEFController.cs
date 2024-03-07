using DotNetCourseWebAPI.Data;
using DotNetCourseWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
