using DotNetCourseWebAPI.Data;
using DotNetCourseWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCourseWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserSalaryController : ControllerBase
    {
        private readonly DataContextDapper _dapper;

        public UserSalaryController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
        }

        [HttpGet("GetSalaries")]
        public IEnumerable<UserSalary> GetSalaries()
        {
            string sql = @"
                SELECT 
                    [UserId],
                    [Salary]
                FROM TutorialAppSchema.UserSalary
            ";
            IEnumerable<UserSalary> userSalaries = _dapper.LoadData<UserSalary>(sql);
            return userSalaries;
        }

    }
}
