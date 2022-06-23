
using CompProgEdu.Core.Features.Requests;
using CompProgEdu.Core.Security;
using CompProgEdu.Features.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CompProgEdu.Features.Controllers
{
    [RoleAuthorization(Roles.GlobalAdmin)]
    [Route("api/sendgrid-api")]
    public class SendGridAPITestController : StandardApiController
    {
        public SendGridAPITestController(IMediator mediator) : base(mediator) { }

        [HttpPost("send-email")]
        public async Task<IActionResult> CreateNew([FromBody] SendToSendGridRequest request)
        {
            await Mediator.Send(request);
            return Ok(); //no idea
        }
    }
}
