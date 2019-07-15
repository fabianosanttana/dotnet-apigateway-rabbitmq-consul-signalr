namespace APIAutenticacao.Controllers
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using ApiAutenticacao.Models;
    using APIAutenticacao.Utils;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private IOptions<Audience> _settings;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;


        public AuthController(IOptions<Audience> settings, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            this._settings = settings;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterView register)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(obj => obj.Errors));
            var user = new IdentityUser
            {
                Email = register.Email,
                UserName = register.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, register.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);
         
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginView user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(obj => obj.Errors));

            var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, false, true);

            if (!result.Succeeded) return BadRequest("Usuário ou senha inválida");
            return Ok(JsonWebToken.GenerateToken(user, _settings));
        }

        [HttpGet]
        public IActionResult Get(string name, string pwd)
        {
            if (name == "user" && pwd == "123")
            {

                var now = DateTime.UtcNow;

                var claims = new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, name),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64)
                };

                var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_settings.Value.Chave));
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    ValidateIssuer = true,
                    ValidIssuer = _settings.Value.Iss,
                    ValidateAudience = true,
                    ValidAudience = _settings.Value.Author,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    RequireExpirationTime = true,

                };

                var jwt = new JwtSecurityToken(
                    issuer: _settings.Value.Iss,
                    audience: _settings.Value.Author,
                    claims: claims,
                    notBefore: now,
                    expires: now.Add(TimeSpan.FromMinutes(2)),
                    signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                );

                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                var responseJson = new
                {
                    access_token = encodedJwt,
                    expires_in = (int)TimeSpan.FromMinutes(2).TotalSeconds
                };

                return Ok(responseJson);
            }
            else
            {
                return Ok("");
            }
        }
    }

    public class Audience
    {
        public string Chave { get; set; }
        public string Iss { get; set; }
        public string Author { get; set; }
    }
}