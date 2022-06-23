using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using FluentValidation;
using MediatR;
using Nancy.Json;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CompProgEdu.Core.Features.Students
{
    public class UploadStudentsFromFileRequest : IRequest<ValidateableResponse<List<StudentDetailDto>>>, IValidateable
    {
        public string StudentList { get; set; }

        public int CourseId { get; set; }
    }

    public class UploadStudentsFromFileRequestHandler : IRequestHandler<UploadStudentsFromFileRequest, ValidateableResponse<List<StudentDetailDto>>>
    {
        private readonly IMediator _mediator;

        public UploadStudentsFromFileRequestHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ValidateableResponse<List<StudentDetailDto>>> Handle(UploadStudentsFromFileRequest request, CancellationToken tkn)
        {
            var js = new JavaScriptSerializer();
            var students = js.Deserialize<List<StudentDetailDto>>(request.StudentList);

            foreach (var student in students)
            {
                await _mediator.Send(new AddStudentToCourseRequest
                {
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    EmailAddress = student.EmailAddress,
                    StudentSchoolNumber = student.StudentSchoolNumber,
                    CourseId = request.CourseId
                }, tkn);
            }


            return new ValidateableResponse<List<StudentDetailDto>>(students);
        }
    }

    public class UploadStudentsFromFileRequestValidation : AbstractValidator<UploadStudentsFromFileRequest>
    {
        private readonly DataContext _dataContext;
        public UploadStudentsFromFileRequestValidation(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
    }
}
