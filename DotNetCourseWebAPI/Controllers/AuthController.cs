using DotNetCourseWebAPI.Data;
using DotNetCourseWebAPI.DTOs;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DotNetCourseWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DataContextDapper _dapper;
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
            _config = config;
        }

        [HttpPost("Register")]
        public IActionResult Register(UserForRegistrationDto userForRegistration)
        {
            if(userForRegistration.Password == userForRegistration.PasswordConfirm)
            {
                string sqlEmailCheck = @"SELECT * FROM 
                                            TutorialAppSchema.Auth 
                                            WHERE Email = '" + userForRegistration.Email + "'";

                IEnumerable<string> existingUsers = _dapper.LoadData<string>(sqlEmailCheck);

                if (existingUsers.Count() == 0)
                {
                    byte[] passwordSalt = new byte[128 / 8];
                    using(RandomNumberGenerator rng = RandomNumberGenerator.Create())
                    {
                        rng.GetNonZeroBytes(passwordSalt);
                    }

                    byte[] passwordHash = GetPasswordHash(passwordSalt, userForRegistration.Password);

                    string sqlAddAuth = @"
                        INSERT INTO TutorialAppSchema.Auth(
                            [Email],
                            [PasswordHash],
                            [PasswordSalt]
                        ) VALUES(
                            '" + userForRegistration.Email + 
                            "', @PasswordHash, @PasswordSalt)";

                    List<SqlParameter> sqlParameters = new List<SqlParameter>();

                    SqlParameter passwordSaltParameter = new SqlParameter("@PasswordSalt", SqlDbType.VarBinary);
                    passwordSaltParameter.Value = passwordSalt;

                    SqlParameter passwordHashParameter = new SqlParameter("@PasswordHash", SqlDbType.VarBinary);
                    passwordHashParameter.Value = passwordHash;

                    sqlParameters.Add(passwordSaltParameter);
                    sqlParameters.Add(passwordHashParameter);

                    if(_dapper.ExecuteSqlWithParameters(sqlAddAuth, sqlParameters))
                    {
                        string sqlAddUser = @"
                            INSERT INTO TutorialAppSchema.Users(
                                [FirstName],
                                [LastName],
                                [Email],
                                [Gender],
                                [Active]) VALUES(" +
                                "'" + userForRegistration.FirstName + "'," +
                                "'" + userForRegistration.LastName + "'," +
                                "'" + userForRegistration.Email + "'," +
                                "'" + userForRegistration.Gender + "', 1)";
                        if(_dapper.ExecuteSql(sqlAddUser))
                            return Ok("Registration completed");

                        throw new Exception("Failed to add user");
                    }
                        

                    throw new Exception("Failed to register user");
                }

                throw new Exception("Email already in use!");
            }

            throw new Exception("Passwords do not match!");
        }

        [HttpPost("Login")]
        public IActionResult Login(UserForLoginDto userForLoginDto)
        {
            string sqlForHashAndSalt = @"SELECT 
                                            [PasswordHash],
                                            [PasswordSalt] 
                                            FROM TutorialAppSchema.Auth 
                                            WHERE Email = '" + userForLoginDto.Email + "'";

            UserForLoginConfirmationDto userForLoginConfirmation = _dapper
                .LoadDataSingle<UserForLoginConfirmationDto>(sqlForHashAndSalt);

            byte[] passwordHash = GetPasswordHash(userForLoginConfirmation.PasswordSalt, userForLoginDto.Password);

            // if(passwordHash == userForLoginConfirmation.PasswordHash) - Won't work, cause they're objects

            for (int i = 0; i < passwordHash.Length; i++)
            {
                if (passwordHash[i] != userForLoginConfirmation.PasswordHash[i])
                    return StatusCode(401, "Incorrect password");
            }

            return Ok("You are now logged in");
        }

        private byte[] GetPasswordHash(byte[] passwordSalt, string password)
        {
            string passwordSaltPlusString = _config.GetSection("AppSettings:PasswordKey").Value +
                            Convert.ToBase64String(passwordSalt);

            return KeyDerivation.Pbkdf2(
                    password: password,
                    salt: Encoding.ASCII.GetBytes(passwordSaltPlusString),
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8
                );
        }

        private string CreateToken(int userId)
        {
            Claim[] claims = new Claim[]
            {
                new Claim("userId", userId.ToString())
            };

            string? tokenKeyString = _config.GetSection("AppSettings:TokenKey").Value;

            SymmetricSecurityKey tokenKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    tokenKeyString != null ? tokenKeyString : ""
                    )
                );

            SigningCredentials credentials = new SigningCredentials(
                    tokenKey, 
                    SecurityAlgorithms.HmacSha512Signature
                );

            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = credentials,
                Expires = DateTime.Now.AddDays(1)
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            
            SecurityToken securityToken = tokenHandler.CreateToken(descriptor);
        }
    }
}
