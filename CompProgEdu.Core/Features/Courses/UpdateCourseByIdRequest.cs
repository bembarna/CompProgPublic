using AutoMapper;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CompProgEdu.Core.Features.Courses
{
    public class UpdateCourseByIdRequest : CourseDto, IRequest<ValidateableResponse<CourseGetDto>>, IValidateable
    {
        public int Id { get; set; }
    }

    public class UpdateCourseByIdRequestHandler : IRequestHandler<UpdateCourseByIdRequest, ValidateableResponse<CourseGetDto>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public UpdateCourseByIdRequestHandler(
            DataContext dataContext,
            IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ValidateableResponse<CourseGetDto>> Handle(UpdateCourseByIdRequest request, CancellationToken tkn)
        {
            var entity = _dataContext.Set<Course>().FirstOrDefault(x => x.Id == request.Id);

            var errorList = new List<ErrorResponse>();

            if (entity == null)
            {
                return new ValidateableResponse<CourseGetDto>($"Course with Id of {request.Id} could not be found!", "Id");
            }

            entity.Title = request.Title;
            entity.Section = request.Section;
            entity.InstructorId = request.InstructorId;

            if (errorList.Any())
            {
                return new ValidateableResponse<CourseGetDto>(errorList);
            }

            var dto = _mapper.Map<CourseGetDto>(entity);

            _dataContext.Update(entity);
            await _dataContext.SaveChangesAsync();

            return new ValidateableResponse<CourseGetDto>(dto);
        }

    }

    public class UpdateCourseByIdRequestValidation : AbstractValidator<UpdateCourseByIdRequest>
    {
        public UpdateCourseByIdRequestValidation()
        {
        }
    }
}
