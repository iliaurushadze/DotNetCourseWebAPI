using DotNetCourseWebAPI.Data;
using DotNetCourseWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCourseWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserJobInfoEFController : ControllerBase
    {
        private readonly DataContextEF _ef;
        public UserJobInfoEFController(IConfiguration config)
        {
            _ef = new DataContextEF(config);
        }

        [HttpGet("GetUserJobInfo")]
        public IEnumerable<UserJobInfo> GetUserJobInfo()
        {
            IEnumerable<UserJobInfo> userJobInfos = _ef.UserJobInfo.ToList();
            return userJobInfos;
        }

        [HttpGet("GetSingleUserJobInfo/{userId}")]
        public UserJobInfo GetSingleUserJobInfo(int userId)
        {
            UserJobInfo? userJobInfo = _ef.UserJobInfo
                .Where(u => u.UserId == userId)
                .FirstOrDefault<UserJobInfo>();

            if (userJobInfo != null)
            {
                return userJobInfo;
            }

            throw new Exception("Failed to get user job info");
        }

        [HttpPut("EditUserJobInfo")]
        public IActionResult EditUserJobInfo(UserJobInfo userJobInfo)
        {
            UserJobInfo? userJobInfoDb = _ef.UserJobInfo
                .Where(u => u.UserId == userJobInfo.UserId)
                .SingleOrDefault<UserJobInfo>();

            if (userJobInfoDb != null)
            {
                userJobInfoDb.JobTitle = userJobInfo.JobTitle;
                userJobInfoDb.Department = userJobInfo.Department;
            }

            if (_ef.SaveChanges() > 0)
                return Ok();

            throw new Exception("Failed to update user job info");
        }
    }
}
