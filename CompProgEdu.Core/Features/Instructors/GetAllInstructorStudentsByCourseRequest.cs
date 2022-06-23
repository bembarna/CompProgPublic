using AutoMapper;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Courses;
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

namespace CompProgEdu.Core.Features.Instructors
{
    public class GetAllInstructorStudentsByCourseRequest : IRequest<ValidateableResponse<List<StudentResponseDto>>>, IValidateable
    {
        public int CourseId { get; set; }
    }

    public class GetAllInstructorStudentsByCourseHandler : IRequestHandler<GetAllInstructorStudentsByCourseRequest, ValidateableResponse<List<StudentResponseDto>>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public GetAllInstructorStudentsByCourseHandler(
            DataContext dataContext,
            IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ValidateableResponse<List<StudentResponseDto>>> Handle(GetAllInstructorStudentsByCourseRequest request, CancellationToken tkn)
        {
            var courseStudents = await _dataContext.Set<Course>()//This can potentially very slow, maybe consider moving it to course
                .Include(x => x.StudentCourses)
                .ThenInclude(x => x.Student)
                .ThenInclude(x => x.User)
                .Where(x => x.Id == request.CourseId)
                .SelectMany(x => x.StudentCourses)
                .Select(x => x.Student).ToListAsync(tkn);

            var studentsDto = _mapper.Map<List<StudentResponseDto>>(courseStudents);

            return new ValidateableResponse<List<StudentResponseDto>>(studentsDto);
        }
    }

    public class GetAllInstructorStudentsByCourseRequestValidation : AbstractValidator<GetAllInstructorStudentsByCourseRequest>
    {
        private readonly DataContext _dataContext;
        public GetAllInstructorStudentsByCourseRequestValidation(DataContext dataContext)
        {
            _dataContext = dataContext;

            RuleFor(x => x.CourseId)
                .EntityMustExist<GetAllInstructorStudentsByCourseRequest, Course>(_dataContext);
        }
    }
}
