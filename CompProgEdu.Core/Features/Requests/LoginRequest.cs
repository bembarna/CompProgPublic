using AutoMapper;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using CompProgEdu.Core.Features.Users;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CompProgEdu.Core.Features.Requests
{
    public class LoginRequest : IRequest<ValidateableResponse<LoginResponse>>, IValidateable, ILoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool StayLoggedIn { get; set; }
    }

    public class LoginRequestHandler : IRequestHandler<LoginRequest, ValidateableResponse<LoginResponse>>
    {
        private readonly UserManager<User> _userManager;

        private readonly SignInManager<User> _signInManager;

        private IHttpContextAccessor _httpContextAccessor;
        public IConfiguration _configuration { get; set; }

        private readonly IMapper _mapper;

        public LoginRequestHandler(IConfiguration configuration, UserManager<User> userManager, SignInManager<User> signInManager, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<ValidateableResponse<LoginResponse>> Handle(LoginRequest request, CancellationToken tkn)
        {
            if(request.Email == null || request.Password == null)
            {
                return new ValidateableResponse<LoginResponse>("Incorrect Email Address or Password!", "Invalid Login");
            }

            var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, request.StayLoggedIn, false);
            if (!result.Succeeded)
            {
                return new ValidateableResponse<LoginResponse>("Incorrect Email Address or Password!", "Invalid Login");
            }

            var user = _userManager.Users.SingleOrDefault(r => r.Email == request.Email);
            var token = await BuildTokenAsync(user, request.StayLoggedIn);

            var role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
            
            var dto = _mapper.Map<UserGetDto>(user);

            dto.Role = role;

            var loginResponse = new LoginResponse
            {
                User = dto,
                Token = token
            };

            return new ValidateableResponse<LoginResponse>(loginResponse);
        }

        private async Task<string> BuildTokenAsync(User user, bool stayLoggedIn)
        {
            DateTime dateTime = stayLoggedIn ? DateTime.UtcNow.AddMonths(1) : DateTime.UtcNow.AddHours(12);
            var roles = await _userManager.GetRolesAsync(user);
            IdentityOptions _options = new IdentityOptions();
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, dateTime.ToString(), ClaimValueTypes.Integer64)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token");

            claimsIdentity.AddClaims(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var token = new JwtSecurityToken
            (
                _configuration["Auth:Token:Issuer"],
                _configuration["Auth:Token:Audience"],
                claimsIdentity.Claims,
                expires: dateTime,
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Auth:Token:Key"])), SecurityAlgorithms.HmacSha256)
            );

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(_options.ClaimsIdentity.RoleClaimType, roles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddHours(1)
            };

            var tokenHandler = new JwtSecurityTokenHandler().WriteToken(token);
            //var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            //var userToken = tokenHandler.WriteToken(securityToken);

            _httpContextAccessor.HttpContext.Session.SetString("JWToken", tokenHandler);

            return tokenHandler;
        }
    }

    public class LoginResponse
    {
        public UserGetDto User { get; set; }
        public string Token { get; set; }
    }

    public class LoginRequestValidation : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidation()
        {
        }
    }

}
