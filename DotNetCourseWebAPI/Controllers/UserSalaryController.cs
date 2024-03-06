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

        [HttpGet("GetSingleSalary/{userId}")]
        public UserSalary GetSalary(int userId)
        {
            string sql = @"
                SELECT
                    [UserId],
                    [Salary]
                FROM TutorialAppSchema.UserSalary
                WHERE UserId = " + userId;
            UserSalary userSalary = _dapper.LoadDataSingle<UserSalary>(sql);
            return userSalary;
        }

        [HttpPut("UpdateUserSalary")]
        public IActionResult UpdateUserSalary(UserSalary userSalary)
        {
            string sql = @"
                UPDATE TutorialAppSchema.UserSalary
                SET
                    Salary = " + userSalary.Salary +
                    "WHERE UserId = " + userSalary.UserId;
            if (_dapper.ExecuteSql(sql))
                return Ok();
            throw new Exception("Failed to update User Salary");
        }
    }
}
