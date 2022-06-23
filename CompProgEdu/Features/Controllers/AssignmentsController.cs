using CompProgEdu.Core.Features.Assignments;
using CompProgEdu.Core.Features.Response;
using CompProgEdu.Core.Security;
using CompProgEdu.Features.Auth;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CompProgEdu.Features.Controllers
{
    [Route("api/assignments")]
    public class AssignmentsController : StandardApiController
    {
        public AssignmentsController(IMediator mediator) : base(mediator) { }

        [HttpGet("by-id/{id}")]
        [RoleAuthorization(Roles.Instructor, Roles.Student)]
        public async Task<ActionResult<ValidateableResponse<AssignmentDetailDto>>> GetById(int id)
        {
            var result = await Mediator.Send(new GetAssignmentByIdRequest { Id = id });

            return result;
        }

        [HttpGet("by-course-id/{courseId}")]
        [RoleAuthorization(Roles.Instructor)]
        public async Task<ActionResult<PaginatedResponse<List<AssignmentSummaryDto>>>> GetAllAssignmentsByCourseId(int courseId, int page, int pageSize)
        {
            var request = await Mediator.Send(new GetAllAssignmentsByCourseIdRequest { CourseId = courseId });

            var paginatedList = request.Result
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToList();

            var result = new PaginatedResponse<List<AssignmentSummaryDto>>(paginatedList)
            {
                TotalCount = request.Result.Count()
            };

            return result;
        }

        [HttpGet("student/{courseId}")]
        [RoleAuthorization(Roles.Student)]
        public async Task<ActionResult<ValidateableResponse<List<AssignmentSummaryDto>>>> GetAllStudentAssignmentsByCourseId(int courseId)
        {
            var result = await Mediator.Send(new GetAllStudentAssignmentsByCourseIdRequest { CourseId = courseId });

            return result;
        }

        [HttpPost]
        [RoleAuthorization(Roles.Instructor)]
        public async Task<ActionResult<ValidateableResponse<AssignmentGetDto>>> Create(
            [FromBody] CreateAssignmentRequest request)
        {
            var result = await Mediator.Send(request);

            return result;
        }

        [HttpPost("assignment-submission")]
        [RoleAuthorization(Roles.Student, Roles.Instructor)]
        public async Task<ActionResult<ValidateableResponse<SubmissionResult>>> SubmitAssignment(
            IFormFile? submission, 
            [FromQuery] int assignmentId, 
            [FromQuery] string? code, 
            [FromQuery] string? language)
        {
            string fileString;
            var fileName = "";

            if (submission != null)
            {
                using var reader = new StreamReader(submission.OpenReadStream()); 
                fileString = await reader.ReadToEndAsync();
                fileName = submission.FileName;
            }
            else
            {
                fileString = code;
            }

            var result = await Mediator.Send(new SubmitAssignmentRequest(fileString, assignmentId, fileName, language));
            return result;
        }

        [HttpPut]
        [RoleAuthorization(Roles.Instructor)]
        public async Task<ActionResult<ValidateableResponse<AssignmentDetailDto>>> Update(
            [FromBody] UpdateAssignmentByIdRequest request)
        {
            var result = await Mediator.Send(request);

            return result;
        }

        [HttpDelete]
        [RoleAuthorization(Roles.Instructor)]
        public async Task<ActionResult<ValidateableResponse<AssignmentGetDto>>> Delete(
            int id)
        {
            var result = await Mediator.Send(new DeleteAssignmentByIdRequest { Id = id });

            return result;
        }
    }
}
