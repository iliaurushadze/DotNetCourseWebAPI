using AutoMapper;
using DotNetCourseWebAPI.Data;
using DotNetCourseWebAPI.DTOs;
using DotNetCourseWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCourseWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserEFController : ControllerBase
    {
        private readonly DataContextEF _ef;
        private readonly IUserRepository _userRepository;
        Mapper _mapper;

        public UserEFController(IConfiguration config, IUserRepository userRepository)
        {
            _ef = new DataContextEF(config);
            _userRepository = userRepository;

            _mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserToAddDto, User>();
            }));
            

        }

        [HttpGet("GetUsers")]
        public IEnumerable<User> GetUsers()
        {
            IEnumerable<User> users = _ef.Users.ToList<User>();
            return users;
        }

        [HttpGet("GetSingleUser/{userId}")]
        public User GetSingleUser(int userId)
        {
            User? user = _ef.Users
                .Where(u => u.UserId == userId)
                .SingleOrDefault<User>();

            if(user != null)
            {
                return user;
            }

            throw new Exception("Failed to get user");
        }

        [HttpPut("EditUser")]
        public IActionResult EditUser(User user)
        {
            User? userDb = _ef.Users
                .Where(u => u.UserId == user.UserId)
                .SingleOrDefault<User>();
            if(userDb != null)
            {
                userDb.FirstName = user.FirstName;
                userDb.LastName = user.LastName;
                userDb.Email = user.Email;
                userDb.Gender = user.Gender;
                userDb.Active = user.Active;
            }

            if (_ef.SaveChanges() > 0)
                return Ok();

            throw new Exception("Failed to update user");
        }

        [HttpPost("AddUser")]
        public IActionResult AddUser(UserToAddDto userToAddDto)
        {
            User userDb = _mapper.Map<User>(userToAddDto);
            
            _ef.Add(userDb);

            if (_ef.SaveChanges() > 0)
                return Ok();

            throw new Exception("Failed to add user");
        }

        [HttpDelete("DeleteUser/{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            User? userDb = _ef.Users
               .Where(u => u.UserId == userId)
               .SingleOrDefault<User>();

            if (userDb != null)
            {
                _ef.Remove(userDb);
            }

            if (_ef.SaveChanges() > 0)
                return Ok();

            throw new Exception("Failed to delete user");
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

        [HttpPost("AddUserJobInfo")]
        public IActionResult AddUserJobInfo(UserJobInfo userJobInfo)
        {
            _ef.Add(userJobInfo);

            if (_ef.SaveChanges() > 0)
                return Ok();

            throw new Exception("Failed to add user job info");
        }

        [HttpDelete("DeleteUserJobInfo{userId}")]
        public IActionResult DeleteUserJobInfo(int userId)
        {
            UserJobInfo? userJobInfo = _ef.UserJobInfo
               .Where(u => u.UserId == userId)
               .SingleOrDefault<UserJobInfo>();

            if (userJobInfo != null)
                _ef.Remove(userJobInfo);

            if (_ef.SaveChanges() > 0)
                return Ok();

            throw new Exception("Failed to delete user job info");
        }

        [HttpGet("GetUserSalary")]
        public IEnumerable<UserSalary> GetUserSalary()
        {
            IEnumerable<UserSalary> userSalaries = _ef.UserSalary.ToList();
            return userSalaries;
        }

        [HttpGet("GetSingleUserSalary/{userId}")]
        public UserSalary GetSingleUserSalary(int userId)
        {
            UserSalary? userSalary = _ef.UserSalary
                .Where(u => u.UserId == userId)
                .FirstOrDefault<UserSalary>();

            if (userSalary != null)
            {
                return userSalary;
            }

            throw new Exception("Failed to get user salary");
        }

        [HttpPut("EditUserSalary")]
        public IActionResult EditUserSalary(UserSalary userSalary)
        {
            UserSalary? userSalaryDb = _ef.UserSalary
                .Where(u => u.UserId == userSalary.UserId)
                .SingleOrDefault<UserSalary>();

            if (userSalaryDb != null)
            {
                userSalaryDb.Salary = userSalary.Salary;
            }

            if (_ef.SaveChanges() > 0)
                return Ok();

            throw new Exception("Failed to update user job info");
        }

        [HttpPost("AddUserSalary")]
        public IActionResult AddUserSalary(UserSalary userSalary)
        {
            _ef.Add(userSalary);

            if (_ef.SaveChanges() > 0)
                return Ok();

            throw new Exception("Failed to add user job info");
        }

        [HttpDelete("DeleteUserSalary{userId}")]
        public IActionResult DeleteUserSalary(int userId)
        {
            UserSalary? userSalary = _ef.UserSalary
               .Where(u => u.UserId == userId)
               .SingleOrDefault<UserSalary>();

            if (userSalary != null)
                _ef.Remove(userSalary);

            if (_ef.SaveChanges() > 0)
                return Ok();

            throw new Exception("Failed to delete user job info");
        }
    }
}
