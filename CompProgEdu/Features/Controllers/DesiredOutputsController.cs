using CompProgEdu.Core.Features.DesiredOutputs;
using CompProgEdu.Core.Features.Response;
using CompProgEdu.Core.Security;
using CompProgEdu.Features.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CompProgEdu.Features.Controllers
{
    [Route("api/desired-outputs")]
    [RoleAuthorization(Roles.Instructor)]
    public class DesiredOutputsController : StandardApiController
    {
        public DesiredOutputsController(IMediator mediator) : base(mediator) {}

        [HttpGet("{id}")]
        public async Task<ActionResult<ValidateableResponse<DesiredOutputGetDto>>> GetById(int id)
        {
            var result = await Mediator.Send(new GetDesiredOutputByIdRequest(id));
            return result;
        }

        [HttpGet("by-assignment/{assignmentId}")]
        public async Task<ActionResult<ValidateableResponse<List<DesiredOutputGetDto>>>> GetByAssignmentId(int assignmentId)
        {
            var result = await Mediator.Send(new GetDesiredOutputsByAssignmentIdRequest(assignmentId));
            return result;
        }

        [HttpPost]
        public async Task<ActionResult<ValidateableResponse<DesiredOutputGetDto>>> Create(
            [FromBody] CreateDesiredOutputRequest request)
        {
            var result = await Mediator.Send(request);
            return result;
        }

        [HttpPut]
        public async Task<ActionResult<ValidateableResponse<DesiredOutputGetDto>>> Update([FromBody] UpdateDesiredOutputRequest request)
        {
            var result = await Mediator.Send(request);
            return result;
        }

        [HttpPut("order")]
        public async Task<ActionResult<ValidateableResponse<List<DesiredOutputGetDto>>>> UpdateOrder([FromBody] UpdateDesiredOutputOrderRequest request)
        {
            var result = await Mediator.Send(request);
            return result;
        }

        [HttpDelete]
        public async Task<ActionResult<ValidateableResponse<DesiredOutputGetDto>>> Delete(int id)
        {
            var result = await Mediator.Send(new DeleteDesiredOutputByIdRequest(id));
            return result;
        }
    }
}
