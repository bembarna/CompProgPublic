using AutoMapper;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Extensions;
using CompProgEdu.Core.Features.Instructors;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CompProgEdu.Core.Features.Courses
{
    public class GetCoursesByInstructorIdRequest : IRequest<ValidateableResponse<List<CourseSummaryDto>>>, IValidateable
    {
        public GetCoursesByInstructorIdRequest(int instructorId)
        {
            InstructorId = instructorId;
        }

        public int InstructorId { get; set; }
    }

    public class GetCoursesByInstructorIdRequestHandler : IRequestHandler<GetCoursesByInstructorIdRequest, ValidateableResponse<List<CourseSummaryDto>>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public GetCoursesByInstructorIdRequestHandler(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ValidateableResponse<List<CourseSummaryDto>>> Handle(GetCoursesByInstructorIdRequest request, CancellationToken cancellationToken)
        {
            var courses = await _dataContext.Set<Course>()
                .Where(x => x.InstructorId == request.InstructorId)
                .Include(x => x.Instructor)
                .ThenInclude(x => x.User)
                .Include(x => x.StudentCourses)
                .ToListAsync(cancellationToken);

            var dtos = _mapper.Map<List<CourseSummaryDto>>(courses);

            return new ValidateableResponse<List<CourseSummaryDto>>(dtos);
        }
    }

    public class GetAllInstructorCourseValidation : AbstractValidator<GetCoursesByInstructorIdRequest>
    {
        private DataContext _dataContext;

        public GetAllInstructorCourseValidation(DataContext dataContext)
        {
            _dataContext = dataContext;

            RuleFor(x => x.InstructorId)
                .EntityMustExist<GetCoursesByInstructorIdRequest, Instructor>(_dataContext);
        }
    }
}
