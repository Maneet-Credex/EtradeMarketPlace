using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using tradeMarketPlace.Models;

namespace tradeMarketPlace.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : ControllerBase
    {
        private readonly TradeMarketPlaceContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContext;

        public UsersController(TradeMarketPlaceContext context, IConfiguration configuration, IHttpContextAccessor httpContext)
        {
            _context = context;
            _configuration = configuration;
            _httpContext = httpContext;
        }

        // GET: api/Users
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        //regiser user
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<User>> Register(User user)
        {
            var passwordHasher = new PasswordHasher<User>();
            user.Password = passwordHasher.HashPassword(user, user.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }
        // login

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<UserLoginResponse>> Login(string email, string password)
        {
            UserLoginResponse response = new UserLoginResponse();
            response.IsSuccess = true;
            response.Message = "Successful";
            try
            {

                // Find the user with the given email
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

                if (user == null)
                {
                    // Return 401 Unauthorized if user not found
                    response.IsSuccess = false;
                    response.Message = "Unauthorized";
                    return response;

                }

                // Verify the password
                var passwordHasher = new PasswordHasher<User>();
                var result = passwordHasher.VerifyHashedPassword(user, user.Password, password);

                if (result == PasswordVerificationResult.Failed)
                {
                    // Return 401 Unauthorized if password is incorrect
                    response.IsSuccess = false;
                    response.Message = "Unauthorized";
                    return response;
                }

                response.Message = "Login Successful";
                response.data = new UserLoginInformation();
                response.data.UserName = user.FirstName;
                response.data.UserEmail = user.Email;
                response.data.Role = user.Type;
                var userId = response.data.UserID = user.UserId.ToString();
                var token = GenerateJwt(response.data.UserID, response.data.UserEmail, response.data.Role);
                response.token = token;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                _httpContext.HttpContext.Session.SetString("token", token);
                _httpContext.HttpContext.Session.SetString("userId", userId);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }
        }

        //private string GenerateJwt(string userID, string userEmail, string role)
        // {
        //    throw new NotImplementedException();
        // }


        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }

        private String GenerateJwt(string UserId, string UserEmail, string Role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Secret"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //claim is used to add identity to JWT token
            var claims = new[] {
         new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
         new Claim(JwtRegisteredClaimNames.Sid, UserId),
         new Claim(JwtRegisteredClaimNames.Email, UserEmail),
         new Claim(ClaimTypes.Role,Role),
         new Claim("Date", DateTime.Now.ToString()),
         };

            var token = new JwtSecurityToken(_configuration["JwtConfig:Issuer"],
              _configuration["JwtConfig:Audiance"],
              claims,    //null original value
              expires: DateTime.Now.AddMinutes(120),

              //notBefore:
              signingCredentials: credentials);

            string Data = new JwtSecurityTokenHandler().WriteToken(token); //return access token 
            return Data;

        }
    }
}
