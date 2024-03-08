
using DotNetCourseWebAPI.Models;

namespace DotNetCourseWebAPI.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContextEF _ef;

        public UserRepository(IConfiguration config)
        {
            _ef = new DataContextEF(config);
        }

        public bool SaveChanges()
        {
            return _ef.SaveChanges() > 0;
        }

        public void AddEntity<T>(T entityToAdd)
        {
            if (entityToAdd != null)
                _ef.Add(entityToAdd);
        }

        public void RemoveEntity<T>(T entityToRemove)
        {
            if(entityToRemove != null)
                _ef.Remove(entityToRemove);
        }

        public IEnumerable<User> GetUsers()
        {
            IEnumerable<User> users = _ef.Users.ToList<User>();
            return users;
        }

        public IEnumerable<UserJobInfo> GetUserJobInfo()
        {
            IEnumerable<UserJobInfo> userJobInfos = _ef.UserJobInfo.ToList();
            return userJobInfos;
        }

        public IEnumerable<UserSalary> GetUserSalary()
        {
            IEnumerable<UserSalary> userSalaries = _ef.UserSalary.ToList();
            return userSalaries;
        }

        public User GetSingleUser(int userId)
        {
            User? user = _ef.Users
                .Where(u => u.UserId == userId)
                .SingleOrDefault<User>();

            if (user != null)
            {
                return user;
            }

            throw new Exception("Failed to get user");
        }

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
    }
}
