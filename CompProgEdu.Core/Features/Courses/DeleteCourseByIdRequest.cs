using AutoMapper;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using CompProgEdu.Core.Features.Students;
using FluentValidation;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CompProgEdu.Core.Features.Courses
{
    public class DeleteCourseByIdRequest : IRequest<ValidateableResponse<CourseGetDto>>, IValidateable
    {
        public int Id { get; set; }
    }

    public class DeleteCourseByIdRequestHandler : IRequestHandler<DeleteCourseByIdRequest, ValidateableResponse<CourseGetDto>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public DeleteCourseByIdRequestHandler(
            DataContext dataContext,
            IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ValidateableResponse<CourseGetDto>> Handle(DeleteCourseByIdRequest request, CancellationToken tkn)
        {
            var entity = _dataContext.Set<Course>().FirstOrDefault(x => x.Id == request.Id);

            if (entity == null)
            {
                return new ValidateableResponse<CourseGetDto>($"Course with Id of {request.Id} could not be found!", "Id");
            }

            var studentCourses = _dataContext.Set<StudentCourse>().Where(x => x.CourseId == request.Id).ToList();

            var dto = _mapper.Map<CourseGetDto>(entity);

            _dataContext.RemoveRange(studentCourses);
            _dataContext.Remove(entity);
            await _dataContext.SaveChangesAsync();

            return new ValidateableResponse<CourseGetDto>(dto);
        }
    }

    public class DeleteCourseByIdRequestValidation : AbstractValidator<DeleteCourseByIdRequest>
    {
        public DeleteCourseByIdRequestValidation()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }
}
