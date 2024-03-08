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
        //private readonly DataContextEF _ef;
        private readonly IUserRepository _userRepository;
        Mapper _mapper;

        public UserEFController(IConfiguration config, IUserRepository userRepository)
        {
            //_ef = new DataContextEF(config);
            _userRepository = userRepository;

            _mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserToAddDto, User>();
            }));
        }

        [HttpGet("GetUsers")]
        public IEnumerable<User> GetUsers()
        {
            return _userRepository.GetUsers();
        }

        [HttpGet("GetSingleUser/{userId}")]
        public User GetSingleUser(int userId)
        {
            return _userRepository.GetSingleUser(userId);
        }

        [HttpPut("EditUser")]
        public IActionResult EditUser(User user)
        {
            User? userDb = _userRepository.GetSingleUser(user.UserId);

            if (userDb != null)
            {
                userDb.FirstName = user.FirstName;
                userDb.LastName = user.LastName;
                userDb.Email = user.Email;
                userDb.Gender = user.Gender;
                userDb.Active = user.Active;
            }

            if (_userRepository.SaveChanges())
                return Ok();

            throw new Exception("Failed to update user");
        }

        [HttpPost("AddUser")]
        public IActionResult AddUser(UserToAddDto userToAddDto)
        {
            User userDb = _mapper.Map<User>(userToAddDto);
            
            _userRepository.AddEntity<User>(userDb);

            if (_userRepository.SaveChanges())
                return Ok();

            throw new Exception("Failed to add user");
        }

        [HttpDelete("DeleteUser/{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            User? userDb = _userRepository.GetSingleUser(userId);

            if (userDb != null)
            {
                _userRepository.RemoveEntity<User>(userDb);
            }

            if (_userRepository.SaveChanges())
                return Ok();

            throw new Exception("Failed to delete user");
        }

        [HttpGet("GetUserJobInfo")]
        public IEnumerable<UserJobInfo> GetUserJobInfo()
        {
            return _userRepository.GetUserJobInfo();
        }

        [HttpGet("GetSingleUserJobInfo/{userId}")]
        public UserJobInfo GetSingleUserJobInfo(int userId)
        {
            return _userRepository.GetSingleUserJobInfo(userId);
        }

        [HttpPut("EditUserJobInfo")]
        public IActionResult EditUserJobInfo(UserJobInfo userJobInfo)
        {
            UserJobInfo? userJobInfoDb = _userRepository.GetSingleUserJobInfo(userJobInfo.UserId);

            if (userJobInfoDb != null)
            {
                userJobInfoDb.JobTitle = userJobInfo.JobTitle;
                userJobInfoDb.Department = userJobInfo.Department;
            }

            if (_userRepository.SaveChanges())
                return Ok();

            throw new Exception("Failed to update user job info");
        }

        [HttpPost("AddUserJobInfo")]
        public IActionResult AddUserJobInfo(UserJobInfo userJobInfo)
        {
            _userRepository.AddEntity<UserJobInfo>(userJobInfo);

            if (_userRepository.SaveChanges())
                return Ok();

            throw new Exception("Failed to add user job info");
        }

        [HttpDelete("DeleteUserJobInfo{userId}")]
        public IActionResult DeleteUserJobInfo(int userId)
        {
            UserJobInfo? userJobInfo = _userRepository.GetSingleUserJobInfo(userId);

            if (userJobInfo != null)
                _userRepository.RemoveEntity<UserJobInfo>(userJobInfo);

            if (_userRepository.SaveChanges())
                return Ok();

            throw new Exception("Failed to delete user job info");
        }

        [HttpGet("GetUserSalary")]
        public IEnumerable<UserSalary> GetUserSalary()
        {
            return _userRepository.GetUserSalary();
        }

        [HttpGet("GetSingleUserSalary/{userId}")]
        public UserSalary GetSingleUserSalary(int userId)
        {
            return _userRepository.GetSingleUserSalary(userId);
        }

        [HttpPut("EditUserSalary")]
        public IActionResult EditUserSalary(UserSalary userSalary)
        {
            UserSalary? userSalaryDb = _userRepository.GetSingleUserSalary(userSalary.UserId);

            if (userSalaryDb != null)
            {
                userSalaryDb.Salary = userSalary.Salary;
            }

            if (_userRepository.SaveChanges())
                return Ok();

            throw new Exception("Failed to update user job info");
        }

        [HttpPost("AddUserSalary")]
        public IActionResult AddUserSalary(UserSalary userSalary)
        {
            _userRepository.AddEntity<UserSalary>(userSalary);

            if (_userRepository.SaveChanges())
                return Ok();

            throw new Exception("Failed to add user job info");
        }

        [HttpDelete("DeleteUserSalary{userId}")]
        public IActionResult DeleteUserSalary(int userId)
        {
            UserSalary? userSalary = _userRepository.GetSingleUserSalary(userId);

            if (userSalary != null)
                _userRepository.RemoveEntity<UserSalary>(userSalary);

            if (_userRepository.SaveChanges())
                return Ok();

            throw new Exception("Failed to delete user job info");
        }
    }
}
