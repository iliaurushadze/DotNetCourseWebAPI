﻿namespace DotNetCourseWebAPI.DTOs
{
    partial class UserForLoginConfirmationDto
    {
        public byte[] PasswordHash { get; set; } = new byte[0];
        public byte[] PasswordSalt { get; set; } = new byte[0];
    }
}
