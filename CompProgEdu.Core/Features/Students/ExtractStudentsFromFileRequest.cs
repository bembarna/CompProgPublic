using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CompProgEdu.Core.Features.Students
{
    public class ExtractStudentsFromFileRequest : StudentDetailDto, IRequest<ValidateableResponse<List<StudentDetailDto>>>, IValidateable
    {
        public IFormFile File { get; set; }
    }

    public class ExtractStudentsFromFileRequestHandler : IRequestHandler<ExtractStudentsFromFileRequest, ValidateableResponse<List<StudentDetailDto>>>
    {
        public async Task<ValidateableResponse<List<StudentDetailDto>>> Handle(ExtractStudentsFromFileRequest request, CancellationToken tkn)
        {
            var students = new List<StudentDetailDto>();

            await using var stream = new MemoryStream();
            await request.File.CopyToAsync(stream, tkn);
            stream.Position = 0;

            using (var reader = new StreamReader(stream, Encoding.ASCII))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    if (line.StartsWith("\"")) continue;
                    
                    var fields = line.Split(',');

                    var student = new StudentDetailDto
                    {
                        FirstName = fields[0],
                        LastName = fields[1],
                        EmailAddress = fields[2],
                        StudentSchoolNumber = fields[3]
                    };

                    students.Add(student);
                }
            }

            return new ValidateableResponse<List<StudentDetailDto>>(students);
        }
    }

    public class ExtractStudentsFromFileRequestValidation : AbstractValidator<ExtractStudentsFromFileRequest>
    {
        private readonly DataContext _dataContext;
        public ExtractStudentsFromFileRequestValidation(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
    }
}
