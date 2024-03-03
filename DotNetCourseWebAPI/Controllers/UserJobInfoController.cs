using DotNetCourseWebAPI.Data;
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
    }
}
