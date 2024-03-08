
using DotNetCourseWebAPI.Models;

namespace DotNetCourseWebAPI.Data
{
    public class UserRepository
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
    }
}
