using CompProgEdu.Core.Features.Courses;
using CompProgEdu.Core.Features.Response;
using CompProgEdu.Core.Security;
using CompProgEdu.Features.Auth;
using CompProgEdu.Features.Courses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CompProgEdu.Features.Controllers
{
    [Route("api/courses")]
    public class CourseController : StandardApiController
    {
        public CourseController(IMediator mediator) : base(mediator) { }

        [HttpGet]
        public async Task<ActionResult<ValidateableResponse<CourseDetailDto>>> GetById(int id)
        {
            var result = await Mediator.Send(new GetCourseByIdRequest {Id = id});

            return result;
        }

        [HttpGet("instructor/{instructorId}")]
        [RoleAuthorization(Roles.Instructor)]
        public async Task<ActionResult<ValidateableResponse<List<CourseSummaryDto>>>> GetAllByInstructorId(int instructorId)
        {
            var result = await Mediator.Send(new GetCoursesByInstructorIdRequest(instructorId));

            return result;
        }

        [HttpGet("student/{studentId}")]
        [RoleAuthorization(Roles.Student)]
        public async Task<ActionResult<ValidateableResponse<List<CourseSummaryDto>>>> GetAllByStudentId(int studentId)
        {
            var result = await Mediator.Send(new GetCoursesByStudentIdRequest(studentId));

            return result;
        }

        [HttpPost]
        [RoleAuthorization(Roles.Instructor)]
        public async Task<ActionResult<ValidateableResponse<CourseGetDto>>> Create(
            [FromBody] CreateCourseRequest request)
        {
            var result = await Mediator.Send(request);

            return result;
        }

        [HttpPut("{id}")]
        [RoleAuthorization(Roles.Instructor)]
        public async Task<ActionResult<ValidateableResponse<CourseGetDto>>> Update(
            [FromBody] CourseDto request, int id)
        {
            var result = await Mediator.Send(new UpdateCourseByIdRequest
            {
                Id = id,
                Title = request.Title,
                Section = request.Section,
                InstructorId = request.InstructorId
            });

            return result;
        }

        [HttpDelete]
        [RoleAuthorization(Roles.Instructor)]
        public async Task<ActionResult<ValidateableResponse<CourseGetDto>>> Delete(
            int id)
        {
            var result = await Mediator.Send(new DeleteCourseByIdRequest { Id = id });

            return result;
        }
    }
}
