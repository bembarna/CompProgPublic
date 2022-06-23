using CompProgEdu.Core.Features.Response;
using CompProgEdu.Core.Features.TestCases;
using CompProgEdu.Core.Security;
using CompProgEdu.Features.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CompProgEdu.Features.Controllers
{
    [Route("api/method-test-case")]
    [RoleAuthorization(Roles.Instructor)]
    public class MethodTestCaseController : StandardApiController
    {
        public MethodTestCaseController(IMediator mediator) : base(mediator) { }

        [HttpGet("{id}")]
        public async Task<ActionResult<ValidateableResponse<MethodTestCaseGetDto>>> GetById(int id)
        {
            var result = await Mediator.Send(new GetMethodTestCaseByIdRequest(id));
            return result;
        }

        [HttpGet("by-assignment/{assignmentId}")]
        public async Task<ActionResult<ValidateableResponse<List<MethodTestCaseGetDto>>>> GetByAssignmentId(int assignmentId)
        {
            var result = await Mediator.Send(new GetMethodTestCaseByAssignmentIdRequest(assignmentId));
            return result;
        }

        [HttpPost]
        public async Task<ActionResult<ValidateableResponse<MethodTestCaseGetDto>>> Create(
            [FromBody] CreateMethodTestCaseRequest request)
        {
            var result = await Mediator.Send(request);
            return result;
        }

        [HttpPut]
        public async Task<ActionResult<ValidateableResponse<MethodTestCaseGetDto>>> Update([FromBody] UpdateMethodTestCaseRequest request)
        {
            var result = await Mediator.Send(request);
            return result;
        }

        [HttpDelete]
        public async Task<ActionResult<ValidateableResponse<MethodTestCaseGetDto>>> Delete(int id)
        {
            var result = await Mediator.Send(new DeleteMethodTestCaseByIdRequest(id));
            return result;
        }
    }
}
