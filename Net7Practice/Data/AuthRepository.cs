using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Net7Practice.Data
{
    public class AuthRepository : IAuthRepository
    {

        private static DataContext _context;
        private static IConfiguration _configuration;
        public AuthRepository(DataContext dataContext, IConfiguration configuration)
        {
            _context = dataContext;
            _configuration = configuration;
        }
        public async Task<ServiceResponse<string>> Login(string UserName, string password)
        {
            var response = new ServiceResponse<string>();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName.Trim().ToLower().Equals(UserName));
            if (user == null)
            {
                response.Issuccess = false;
                response.Message = "something went wrong while login";
                return response;
            }
            else if (!VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
            {
                response.Issuccess = false;
                response.Message = "wrong password";
                return response;
            }
            else
            {
                response.Data = CreateToken(user);
            }
            return response;
        }

        public async Task<ServiceResponse<int>> Register(Users users, string password)
        {
            var response = new ServiceResponse<int>();
            if (await UserExists(users.UserName))
            {
                response.Issuccess = false;
                response.Message = "User already exsist";
                return response;
            }
            CreatePasswordHash(password, out byte[] passwordhasher, out byte[] passwordsalt);
            users.PasswordHash = passwordhasher;
            users.PasswordSalt = passwordsalt;
            _context.Users.Add(users);
            await _context.SaveChangesAsync();
            response.Data = users.Id; ;
            return response;

        }

        public async Task<bool> UserExists(string UserName)
        {
            if (await _context.Users.AnyAsync(x => x.UserName.Trim().ToLower() == UserName.Trim().ToLower()))
            {
                return true;
            }
            return false;
        }
        private void CreatePasswordHash(string Password, out byte[] PasswordHasher, out byte[] passwordsalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordsalt = hmac.Key;
                PasswordHasher = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Password));

            }
        }

        private bool VerifyPassword(string Password, byte[] passwordHasher, byte[] passwordsalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordsalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Password));
                return computedHash.SequenceEqual(passwordHasher);
            }
        }


        private string CreateToken(Users users)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,users.Id.ToString()),
                new Claim(ClaimTypes.Name,users.UserName)
            };

            var appsettingtoken = _configuration.GetSection("AppSettings:Token").Value;
            if (appsettingtoken is null)
                throw new Exception("Appsetting token is null");

            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(appsettingtoken));
            SigningCredentials cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = cred
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

            
        }
    }
}
