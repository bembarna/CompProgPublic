using CompProgEdu.Core.Features.CurlySets;
using CompProgEdu.Core.Features.Response;
using CompProgEdu.Core.Features.Submissions;
using CompProgEdu.Core.Security;
using CompProgEdu.Features.Auth;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace CompProgEdu.Features.Controllers
{
    [RoleAuthorization(Roles.Instructor)]
    [Route("api/instructor-submissions")]
    public class InstructorSubmissionsController : StandardApiController
    {
        public InstructorSubmissionsController(IMediator mediator) : base(mediator) { }

        [HttpPost]
        public async Task<ActionResult<ValidateableResponse<InstructorSubmissionDetailDto>>> Create(IFormFile codeFile, [FromQuery] int assignmentId)
        {
            using var reader = new StreamReader(codeFile.OpenReadStream());
            var code = await reader.ReadToEndAsync();

            var result = await Mediator.Send(new BuildCodeBlockRequest(assignmentId, code, codeFile.FileName));

            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ValidateableResponse<InstructorSubmissionDetailDto>>> GetById(int id)
        {
            var result = await Mediator.Send(new GetInstructorSubmissionRequest { Id = id });

            return result;
        }

        [HttpGet("child-{id}")]
        public async Task<ActionResult<ValidateableResponse<CurlySetDetailDto>>> GetChildById(int id)
        {
            var result = await Mediator.Send(new GetCurlyChildByIdRequest { Id = id });

            return result;
        }

        [HttpGet("tree-node")]
        public async Task<ActionResult<ValidateableResponse<List<TreeNodeDto>>>> GetTreeNodes(int assignmentId)
        {
            var result = await Mediator.Send(new GetTreeNodesByAssignmentIdRequest(assignmentId));

            return result;
        }

        [HttpDelete]
        public async Task<ActionResult<ValidateableResponse<InstructorSubmissionDetailDto>>> DeleteByAssignmentId(int assignmentId)
        {
            var result = await Mediator.Send(new DeleteInstructorSubmissionByAssignmentIdRequest(assignmentId));

            return result;
        }
    }
}