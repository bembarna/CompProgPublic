using AutoMapper;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Extensions;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using CompProgEdu.Core.Features.Students;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CompProgEdu.Core.Features.Courses
{
    public class GetCoursesByStudentIdRequest : IRequest<ValidateableResponse<List<CourseSummaryDto>>>, IValidateable
    {
        public GetCoursesByStudentIdRequest(int studentId)
        {
            StudentId = studentId;
        }

        public int StudentId { get; set; }
    }

    public class GetCoursesByStudentIdRequestHandler : IRequestHandler<GetCoursesByStudentIdRequest, ValidateableResponse<List<CourseSummaryDto>>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public GetCoursesByStudentIdRequestHandler(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ValidateableResponse<List<CourseSummaryDto>>> Handle(GetCoursesByStudentIdRequest request, CancellationToken cancellationToken)
        {
            var courses = await _dataContext.Set<StudentCourse>()
                .Where(x => x.StudentId == request.StudentId)
                .Include(x => x.Course)
                .ThenInclude(x => x.Instructor)
                .ThenInclude(x => x.User)
                .Select(x => x.Course)
                .ToListAsync(cancellationToken);

            var dtos = _mapper.Map<List<CourseSummaryDto>>(courses);

            return new ValidateableResponse<List<CourseSummaryDto>>(dtos);
        }
    }

    public class GetAllStudentCourseValidation : AbstractValidator<GetCoursesByStudentIdRequest>
    {
        private DataContext _dataContext;

        public GetAllStudentCourseValidation(DataContext dataContext)
        {
            _dataContext = dataContext;

            RuleFor(x => x.StudentId)
                .EntityMustExist<GetCoursesByStudentIdRequest, Student>(_dataContext);
        }
    }
}
