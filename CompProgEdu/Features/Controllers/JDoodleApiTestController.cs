using CompProgEdu.Core.Features.Request;
using CompProgEdu.Core.Features.Requests;
using CompProgEdu.Core.Features.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CompProgEdu.Features.Controllers
{
    [Route("api/jdoodle-api")]
    public class JDoodleApiTestController : Controller
    {
        public IMediator Mediator { get; set; }

        public JDoodleApiTestController(IMediator mediator)
        {
            Mediator = mediator;
        }


        [HttpPost("test-code-exe")]
        public async Task<ActionResult<ValidateableResponse<JDoodleRequest>>> CreateNew([FromBody] SendToJDoodleApiRequest request)
        {
            var result = await Mediator.Send(request);

            return result;
        }
    }
}
