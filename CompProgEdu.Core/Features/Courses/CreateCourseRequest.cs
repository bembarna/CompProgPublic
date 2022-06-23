using AutoMapper;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Courses;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CompProgEdu.Features.Courses
{
    public class CreateCourseRequest : CourseDto, IRequest<ValidateableResponse<CourseGetDto>>, IValidateable
    {
        //public List<int> StudentIds { get; set; }
    }

    public class CreateCourseRequestHandler : IRequestHandler<CreateCourseRequest, ValidateableResponse<CourseGetDto>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public CreateCourseRequestHandler(
            DataContext dataContext,
            IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ValidateableResponse<CourseGetDto>> Handle(CreateCourseRequest request, CancellationToken tkn)
        {
            var entity = new Course
            {
                Title = request.Title,
                Section = request.Section,
                InstructorId = request.InstructorId
            };

            //if (request.StudentIds.Any())
            //{
            //    /*TODO: This will be similar to how I did my AddStudentToCourse endpoint, it will be a separate request that takes in a list of emails, 
            //    if they exist, get their Id and add it to the list, if it doesnt exist, create the user/student then add the new Id  to the list
            //    and finally send the new student an email telling them they have been registered with a temp password, the sent email logic should be done in createStudentREQUEST*/
            //    entity.StudentCourses = request.StudentIds.Select(x => new StudentCourse
            //    {
            //        Course = entity,
            //        StudentId = x
            //    }).ToList();
            //}

            await _dataContext.AddAsync(entity);
            await _dataContext.SaveChangesAsync();

            var dto = _mapper.Map<CourseGetDto>(entity);

            return new ValidateableResponse<CourseGetDto>(dto);
        }
    }

    public class CreateCourseRequestValidation : AbstractValidator<CreateCourseRequest>
    {
        public CreateCourseRequestValidation(IValidator<CourseDto> mainDtoValidator)
        {
            Include(mainDtoValidator);
        }
    }
}