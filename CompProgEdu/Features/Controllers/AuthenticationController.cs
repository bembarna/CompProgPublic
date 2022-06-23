using AutoMapper;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.CurlySets;
using CompProgEdu.Core.Features.Instructors;
using CompProgEdu.Core.Features.Request;
using CompProgEdu.Core.Features.Requests;
using CompProgEdu.Core.Features.Response;
using CompProgEdu.Core.Features.Students;
using CompProgEdu.Core.Features.Submissions;
using CompProgEdu.Core.Features.Users;
using CompProgEdu.Core.Security;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CompProgEdu.Features.Controllers
{
    [Route("api/authenticate")]
    public class AuthenticationController : Controller
    {
        public IMediator Mediator { get; set; }

        private readonly SignInManager<User> _signInManager;

        private readonly UserManager<User> _userManager;

        private readonly IMapper _mapper;

        private readonly DataContext _dataContext;

        public AuthenticationController(IMediator mediator, SignInManager<User> signInManager, UserManager<User> userManager, IMapper mapper, DataContext dataContext)
        {
            Mediator = mediator;
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            _dataContext = dataContext;
        }

        [HttpPost("login")]
        public async Task<ValidateableResponse<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            var result = await Mediator.Send(request);

            return result;
        }

        [HttpPost("register")]//TODO make this create a user with no Role and set their "isEmailConfirmed" to false and have the request send off a sendgrid email that sends a link to "Complete registration"(more on this will need to be talked about later with clinton)
        public async Task<ValidateableResponse<UserGetDto>> Register([FromBody] RegisterRequest request)
        {
            var result = await Mediator.Send(request);

            return result;
        }

        [HttpPost("register-student")]//TODO make this create a user with no Role and set their "isEmailConfirmed" to false and have the request send off a sendgrid email that sends a link to "Complete registration"(more on this will need to be talked about later with clinton)
        public async Task<ValidateableResponse<UserGetDto>> RegisterStudent([FromBody] RegisterStudentRequest request)
        {
            var result = await Mediator.Send(request);

            return result;
        }

        [HttpPost("register-instructor")]//TODO make this create a user with no Role and set their "isEmailConfirmed" to false and have the request send off a sendgrid email that sends a link to "Complete registration"(more on this will need to be talked about later with clinton)
        public async Task<ValidateableResponse<UserGetDto>> RegisterInstructor([FromBody] RegisterInstructorRequest request)
        {
            var result = await Mediator.Send(request);

            return result;
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.SetString("JWToken", string.Empty);

            return Ok();
        }

        [HttpPost("get-me")]
        public async Task<ValidateableResponse<UserGetMeDto>> GetMe()
        {
            string username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(username == null)
            {
                return new ValidateableResponse<UserGetMeDto>("Error user not found!", "User");
            }

            User user = await _userManager.FindByNameAsync(username);

            var role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();

            var dto = _mapper.Map<UserGetMeDto>(user);

            dto.Role = role;

            switch (role)
            {
                case Roles.Instructor:
                {
                    var instructor = _dataContext.Set<Instructor>().First(x => x.User.Id == user.Id);
                    dto.InstructorId = instructor.Id;
                }
                    break;
                case Roles.Student:
                {
                    var student = _dataContext.Set<Student>().First(x => x.User.Id == user.Id);
                    dto.StudentId = student.Id;
                }
                    break;

            }

            return new ValidateableResponse<UserGetMeDto>(dto);
        }
    }
}
