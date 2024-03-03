using DotNetCourseWebAPI.Data;
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
        private DataContextDapper _dapper;
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
            Console.WriteLine(sql);
            User user = _dapper.LoadDataSingle<User>(sql);
            return user;
        }

        [HttpGet("TestConnection")]
        public DateTime GetDate()
        {
            return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
        }

        [HttpGet("GetUsers/{testValue}")]
        public string[] GetUsers(string testValue)
        {
            string[] resultArray = new string[] {
                "test1",
                "test2",
                testValue
            };
            return resultArray;
        }
    }
}
