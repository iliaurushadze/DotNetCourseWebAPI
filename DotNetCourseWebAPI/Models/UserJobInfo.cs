namespace DotNetCourseWebAPI.Models
{
    public partial class UserJobInfo
    {
        public int UserId { get; set; }
        public string JobTytle { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
    }
}
