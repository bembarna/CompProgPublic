using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CompProgEdu.Features.Controllers
{
    //TODO: Authenticate Here once auth is in!
    [AuthenticateApi]
    public class StandardApiController : Controller
    {
        public IMediator Mediator { get; set; }

        public StandardApiController(IMediator mediator)
        {
            Mediator = mediator;
        }
    }
}
