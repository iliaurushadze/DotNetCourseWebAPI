using DotNetCourseWebAPI.Data;
using DotNetCourseWebAPI.DTOs;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
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
        public IActionResult Register(UserForRegistrationDto userForRegistrationDto)
        {
            if(userForRegistrationDto.Password == userForRegistrationDto.PasswordConfirm)
            {
                string sqlEmailCheck = @"SELECT * FROM 
                                            TutorialAppSchema.Auth 
                                            WHERE Email = '" + userForRegistrationDto.Email + "'";

                if (_dapper.ExecuteSqlWithRowCount(sqlEmailCheck) == 0)
                {
                    byte[] passwordSalt = new byte[128 / 8];
                    using(RandomNumberGenerator rng = RandomNumberGenerator.Create())
                    {
                        rng.GetNonZeroBytes(passwordSalt);
                    }

                    byte[] passwordHash = GetPasswordHash(passwordSalt, userForRegistrationDto.Password);

                    string sqlAddAuth = @"
                        INSERT INTO TutorialAppSchema.Auth(
                            [Email],
                            [PasswordHash],
                            [PasswordSalt]
                        ) VALUES(
                            '" + userForRegistrationDto.Email + 
                            "', @PasswordHash, @PasswordSalt)";

                    List<SqlParameter> sqlParameters = new List<SqlParameter>();

                    SqlParameter passwordSaltParameter = new SqlParameter("@PasswordSalt", SqlDbType.VarBinary);
                    passwordSaltParameter.Value = passwordSalt;

                    SqlParameter passwordHashParameter = new SqlParameter("@PasswordHash", SqlDbType.VarBinary);
                    passwordHashParameter.Value = passwordHash;

                    sqlParameters.Add(passwordSaltParameter);
                    sqlParameters.Add(passwordHashParameter);

                    if(_dapper.ExecuteSqlWithParameters(sqlAddAuth, sqlParameters))
                        return Ok();

                    throw new Exception("Failed to register user");
                }

                throw new Exception("Email already in use!");
            }

            throw new Exception("Passwords do not match!");
        }

        [HttpPost("Login")]
        public IActionResult Login(UserForLoginDto userForLoginDto)
        {
            string sqlForHashAndSaalt = @"SELECT 
                                            [PasswordHash],
                                            [PasswordSalt] 
                                            FROM TutorialAppSchema.Auth 
                                            WHERE Email = '" + userForLoginDto.Email + "'";

            UserForLoginConfirmationDto userForLoginConfirmation = _dapper
                .LoadDataSingle<UserForLoginConfirmationDto>(sqlForHashAndSaalt);

            byte[] passwordHash = GetPasswordHash(userForLoginConfirmation.PasswordSalt, userForLoginDto.Password);

            // if(passwordHash == userForLoginConfirmation.PasswordHash) - Won't work, cause they're objects
            for (int i = 0; i < passwordHash.Length; i++)
            {
                if (passwordHash[i] != userForLoginConfirmation.PasswordHash[i])
                    return StatusCode(401, "Incorrect password");
            }

            return Ok();
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
    }
}
