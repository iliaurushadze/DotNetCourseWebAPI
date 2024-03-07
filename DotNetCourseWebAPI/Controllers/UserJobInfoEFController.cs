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

        
    }
}
