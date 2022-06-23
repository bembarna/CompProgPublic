using CompProgEdu.Core.Features.Instructors;
using CompProgEdu.Core.Features.Response;
using CompProgEdu.Core.Features.Students;
using CompProgEdu.Core.Security;
using CompProgEdu.Features.Auth;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompProgEdu.Features.Controllers
{
    [RoleAuthorization(Roles.Instructor)]
    [Route("api/instructors")]
    public class InstructorsController : StandardApiController
    {
        public InstructorsController(IMediator mediator) : base(mediator) {}

        [RoleAuthorization(Roles.GlobalAdmin)]
        [HttpPost]
        public async Task<ActionResult<ValidateableResponse<InstructorResponseDto>>> Create(//TODO make this create a user that isnt validated or confirmed, but sends them an email with a temp password and a link to confirm that user(more on this will need to be talked about later with clinton)
            [FromBody] CreateInstructorRequest request)
        {
            var result = await Mediator.Send(request);

            return result;
        }

        [HttpPost("add-student-to-course")]
        public async Task<ActionResult<ValidateableResponse<StudentResponseDto>>> AddStudentToCourse(
            [FromBody] AddStudentToCourseRequest request)
        {
            var result = await Mediator.Send(request);

            return result;
        }

        [HttpPost("extract")]
        public async Task<ActionResult<ValidateableResponse<List<StudentDetailDto>>>> ExtractFromFile(
            IFormFile file)
        {
            var request = new ExtractStudentsFromFileRequest
            {
                File = file
            };

            var result = await Mediator.Send(request);

            return result;
        }

        [HttpPost("upload")]
        public async Task<ActionResult<ValidateableResponse<List<StudentDetailDto>>>> AddStudentsFromFile(UploadStudentsFromFileRequest request)
        {
            var result = await Mediator.Send(request);

            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ValidateableResponse<InstructorResponseDto>>> GetById(
        int id)
        {
            var result = await Mediator.Send(new GetInstructorByIdRequest { Id = id });

            return result;
        }

        [HttpGet("all-course-students/{courseId}")]
        public async Task<ActionResult<PaginatedResponse<List<StudentResponseDto>>>> GetCourseStudents(int courseId, int page, int pageSize)
        {
            var request = await Mediator.Send(new GetAllInstructorStudentsByCourseRequest {CourseId = courseId });

            var paginatedList = request.Result
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToList();

            var result = new PaginatedResponse<List<StudentResponseDto>>(paginatedList)
            {
                TotalCount = request.Result.Count()
            };

            return result;
        }

        [RoleAuthorization(Roles.GlobalAdmin)]
        [HttpGet]
        public async Task<ActionResult<ValidateableResponse<List<InstructorGetAllResponseDto>>>> GetAll()
        {
            var result = await Mediator.Send(new GetAllInstructorsRequest());

            return result;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ValidateableResponse<InstructorResponseDto>>> Update(
        [FromBody] InstructorDetailDto request, int id)
        {
            var result = await Mediator.Send(new UpdateInstructorRequest
            {
                Id = id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                EmailAddress = request.EmailAddress,
                Title = request.Title
            });

            return result;
        }

        [RoleAuthorization(Roles.GlobalAdmin)]
        [HttpDelete]
        public async Task<ActionResult<ValidateableResponse<InstructorResponseDto>>> Delete(
        int id)
        {
            var result = await Mediator.Send(new DeleteInstructorRequest { Id = id });

            return result;
        }
    }
}
