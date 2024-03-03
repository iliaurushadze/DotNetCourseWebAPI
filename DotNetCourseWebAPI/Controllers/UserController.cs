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
    }
}
