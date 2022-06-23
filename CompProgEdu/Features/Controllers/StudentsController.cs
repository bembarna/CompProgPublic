using CompProgEdu.Core.Features.Response;
using CompProgEdu.Core.Features.Students;
using CompProgEdu.Core.Security;
using CompProgEdu.Features.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CompProgEdu.Features.Controllers
{
    [Route("api/students")]
    public class StudentsController : StandardApiController
    {
        public StudentsController(IMediator mediator) : base(mediator) { }

        [HttpPost]
        public async Task<ActionResult<ValidateableResponse<StudentResponseDto>>> Create(
            [FromBody] CreateStudentRequest request)
        {
            var result = await Mediator.Send(request);

            return result;
        }

        [RoleAuthorization(Roles.Student, Roles.Instructor)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ValidateableResponse<StudentResponseDto>>> GetById(
        int id)
        {
            var result = await Mediator.Send(new GetStudentByIdRequest { Id = id });

            return result;
        }

        [HttpGet]
        public async Task<ActionResult<ValidateableResponse<List<StudentGetAllResponseDto>>>> GetAll()
        {
            var result = await Mediator.Send(new GetAllStudentsRequest());

            return result;
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult<ValidateableResponse<StudentResponseDto>>> Update(
        [FromBody] StudentUpdateDto request, int id)
        {
            var result = await Mediator.Send(new UpdateStudentRequest
            {
                Id = id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                StudentSchoolNumber = request.StudentSchoolNumber
            });

            return result;
        }

        [HttpPut("{id}/update-password")]
        public async Task<ActionResult<ValidateableResponse<StudentResponseDto>>> UpdatePassword(
            [FromBody] UpdateStudentPasswordRequest request, int id)
        {
            request.Id = id;
            var result = await Mediator.Send(request);

            return result;
        }

        [HttpDelete]
        public async Task<ActionResult<ValidateableResponse<StudentResponseDto>>> Delete(
        int id)
        {
            var result = await Mediator.Send(new DeleteStudentRequest { Id = id });

            return result;
        }
    }
}
