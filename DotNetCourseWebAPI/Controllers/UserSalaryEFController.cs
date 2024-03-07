using DotNetCourseWebAPI.Data;
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


    }
}
