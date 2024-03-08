using DotNetCourseWebAPI.Data;
using DotNetCourseWebAPI.DTOs;
using DotNetCourseWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml;

namespace DotNetCourseWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContextDapper _dapper;
        public UserController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
        }

        [HttpGet("GetUsers")]
        public IEnumerable<User> GetUsers()
        {
            string sql = @"
            SELECT [UserId]
                ,[FirstName]
                ,[LastName]
                ,[Email]
                ,[Gender]
                ,[Active]
            FROM [DotNetCourseDatabase].[TutorialAppSchema].[Users]";

            IEnumerable<User> users = _dapper.LoadData<User>(sql);
            return users;
        }

        [HttpGet("GetSingleUser/{userId}")]
        public User GetSingleUser(int userId)
        {
            string sql = @"
            SELECT [UserId]
                ,[FirstName]
                ,[LastName]
                ,[Email]
                ,[Gender]
                ,[Active]
            FROM [DotNetCourseDatabase].[TutorialAppSchema].[Users]
            WHERE UserId = " + userId.ToString();
            User user = _dapper.LoadDataSingle<User>(sql);
            return user;
        }

        [HttpPut("EditUser")]
        public IActionResult EditUser(User user)
        {
            string sql = @"
                UPDATE [DotNetCourseDatabase].[TutorialAppSchema].[Users]
                    SET FirstName = " + "'" + user.FirstName + "'," +
                        "LastName = " + "'" + user.LastName + "'," +
                        "Email = " + "'" + user.Email + "'," +
                        "Gender = " + "'" + user.Gender + "'," +
                        "Active = " + "'" + user.Active + "'" +
                "WHERE UserId = " + user.UserId;  

            if(_dapper.ExecuteSql(sql))
            {
                return Ok(user);
            }

            throw new Exception("Failed to update user");
        }

        [HttpPost("AddUser")]
        public IActionResult AddUser(UserToAddDto userToAddDto)
        {
            string sql = @"
            INSERT INTO TutorialAppSchema.Users(
                [FirstName],
                [LastName],
                [Email],
                [Gender],
                [Active]) VALUES("+ 
                "'" + userToAddDto.FirstName + "'," +
                "'" + userToAddDto.LastName + "'," +
                "'" + userToAddDto.Email + "'," +
                "'" + userToAddDto.Gender + "'," +
                "'" + userToAddDto.Active + "'" +
            ")";
            Console.WriteLine(sql);

            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to add user");
        }

        [HttpDelete("DeleteUser/{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            string sql = @"
                DELETE FROM TutorialAppSchema.Users 
                    WHERE UserId = " + userId.ToString();

            if (_dapper.ExecuteSql(sql))
                return Ok();

            throw new Exception("Failed to delete user");
        }

        [HttpGet("GetUserJobInfo")]
        public IEnumerable<UserJobInfo> GetUserJobInfo()
        {
            string sql = @"
                SELECT [UserId]
                    ,[JobTitle]
                    ,[Department]
                FROM [DotNetCourseDatabase].[TutorialAppSchema].[UserJobInfo]
                ";
            IEnumerable<UserJobInfo> userJobInfos = _dapper.LoadData<UserJobInfo>(sql);
            return userJobInfos;
        }

        [HttpGet("GetSingleUserJobInfo/{userId}")]
        public UserJobInfo GetSingleUserJobInfo(int userId)
        {
            string sql = @"
                SELECT [UserId]
                    ,[JobTitle]
                    ,[Department]
                FROM [DotNetCourseDatabase].[TutorialAppSchema].[UserJobInfo]
                WHERE UserId = " + userId.ToString();
            UserJobInfo userJobInfo = _dapper.LoadDataSingle<UserJobInfo>(sql);
            return userJobInfo;
        }

        [HttpPut("EditUserJobInfo")]
        public IActionResult EditUserJobInfo(UserJobInfo userJobInfo)
        {
            string sql = @"
                UPDATE [DotNetCourseDatabase].[TutorialAppSchema].[UserJobInfo]
                    SET JobTitle = " + "'" + userJobInfo.JobTitle + "'," +
                        "Department = " + "'" + userJobInfo.Department + "'" +
                "WHERE UserId = " + userJobInfo.UserId;

            if (_dapper.ExecuteSql(sql))
                return Ok();

            throw new Exception("Failed to update user job info");
        }

        [HttpPost("AddUserJobInfo")]
        public IActionResult AdduserJobInfo(UserJobInfo userJobInfo)
        {
            string sql = @"
                INSERT INTO TutorialAppSchema.UserJobInfo(
                    [UserId],
                    [JobTitle],
                    [Department]) VALUES(" +
                        "'" + userJobInfo.UserId + "'," +
                        "'" + userJobInfo.JobTitle + "'," +
                        "'" + userJobInfo.Department + "'" +
                ")";
            Console.WriteLine(sql);

            if (_dapper.ExecuteSql(sql))
                return Ok();

            throw new Exception("Failed to add user job info");
        }

        [HttpDelete("DeleteUserJobInfo/{userId}")]
        public IActionResult DeleteUserJobInfo(int userId)
        {
            string sql = @"
                DELETE FROM TutorialAppSchema.UserJobInfo
                WHERE UserId = " + userId.ToString();
            if (_dapper.ExecuteSql(sql))
                return Ok();

            throw new Exception("Failed to delete user job info");
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

        [HttpPost("AddUserSalary")]
        public IActionResult AddUserSalary(UserSalary userSalary)
        {
            string sql = @"
                INSERT INTO TutorialAppSchema.UserSalary
                (UserId, Salary)
                VALUES("
                    + userSalary.UserId + ","
                    + userSalary.Salary +
                ")";
            if (_dapper.ExecuteSql(sql))
                return Ok();
            throw new Exception("Failed to add User Salary");
        }

        [HttpDelete("DeleteUserSalary/{userId}")]
        public IActionResult DeleteUserSalary(int userId)
        {
            string sql = @"
                DELETE FROM TutorialAppSchema.UserSalary
                WHERE UserId = " + userId;
            Console.WriteLine(sql);
            if (_dapper.ExecuteSql(sql))
                return Ok();
            throw new Exception("Failed to delete user salary");
        }
    }
}
