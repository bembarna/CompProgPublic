using CompProgEdu.Core.Features.Response;
using CompProgEdu.Core.Features.Users;
using CompProgEdu.Core.Security;
using CompProgEdu.Features.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CompProgEdu.Features.Controllers
{
    [RoleAuthorization(Roles.GlobalAdmin)]
    [Route("api/users")]
    public class UsersController : StandardApiController
    {
        public UsersController(IMediator mediator) : base(mediator) { }

        [HttpPost]
        public async Task<ActionResult<ValidateableResponse<UserGetDto>>> Create(//TODO make this create a user that isnt validated or confirmed, but sends them an email with a temp password and a link to confirm that user(more on this will need to be talked about later with clinton)
            [FromBody] CreateUserRequest request)
        {
            var result = await Mediator.Send(request);

            return result;
        }

        [HttpGet]
        public async Task<ActionResult<ValidateableResponse<UserGetDto>>> GetById(
        int id)
        {
            var result = await Mediator.Send(new GetUserByIdRequest { Id = id });

            return result;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ValidateableResponse<UserGetDto>>> Update(int id,
        [FromBody] UserDto request)
        {
            var result = await Mediator.Send((new UpdateUserRequest { 
                Id = id,
                EmailAddress = request.EmailAddress,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Role = request.Role
            }));

            return result;
        }

        //[HttpPut("{id}/update-password")]
        //public async Task<ActionResult<ValidateableResponse<UserGetDto>>> UpdatePassword(int id,
        //    [FromBody] UpdateUserPasswordRequest request)
        //{
        //    request.Id = id;
        //    var result = await Mediator.Send(request);

        //    return result;
        //}

        [HttpDelete]
        public async Task<ActionResult<ValidateableResponse<UserGetDto>>> Delete(
        int id)
        {
            var result = await Mediator.Send(new DeleteUserByIdRequest { Id = id });

            return result;
        }
    }
}
