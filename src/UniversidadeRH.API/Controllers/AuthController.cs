using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace UniversidadeRH.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Tags("00. Autenticação e Acesso")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<IdentityUser> userManager, 
                              SignInManager<IdentityUser> signInManager, 
                              IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("registrar")]
        public async Task<ActionResult> Registrar(RegisterUserDto registerUser)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = new IdentityUser
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, registerUser.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Ok(GerarTokenJwt(user));
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginUserDto loginUser)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(loginUser.Email);
                if (user != null)
                {
                    return Ok(GerarTokenJwt(user));
                }
            }

            return BadRequest("Usuário ou senha inválidos");
        }

        private string GerarTokenJwt(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            
            // OBTÉM A CHAVE DO APPSETTINGS OU USA A PADRÃO (Mesma lógica do Program.cs)
            var keyString = _configuration["Jwt:Key"] ?? "MinhaChaveSuperSecretaDeDesenvolvimento2026!";
            var key = Encoding.ASCII.GetBytes(keyString);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                    new Claim(ClaimTypes.Email, user.Email ?? string.Empty)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"] ?? "UniversidadeRH",
                Audience = _configuration["Jwt:Audience"] ?? "UniversidadeRH"
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

    // ==========================================
    // DTOs (Data Transfer Objects)
    // ==========================================
    
    public class RegisterUserDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public class LoginUserDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}