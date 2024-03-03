using DotNetCourseWebAPI.Data;
using DotNetCourseWebAPI.DTOs;
using DotNetCourseWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCourseWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserJobInfoController : ControllerBase
    {
        private readonly DataContextDapper _dapper;

        public UserJobInfoController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
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
                WHERE UserId = " +  userId.ToString();
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

            if(_dapper.ExecuteSql(sql))
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
    }
}
