using AutoMapper;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CompProgEdu.Core.Features.Courses
{
    public class GetCourseByIdRequest : IRequest<ValidateableResponse<CourseDetailDto>>, IValidateable
    {
        public int Id { get; set; }
    }

    public class GetCourseByIdRequestHandler : IRequestHandler<GetCourseByIdRequest, ValidateableResponse<CourseDetailDto>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public GetCourseByIdRequestHandler(
            DataContext dataContext,
            IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ValidateableResponse<CourseDetailDto>> Handle(GetCourseByIdRequest request, CancellationToken tkn)
        {
            var entity = await _dataContext.Set<Course>()
                .Where(x => x.Id == request.Id)
                .Include(x => x.Instructor)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                return new ValidateableResponse<CourseDetailDto>($"Course with Id of {request.Id} could not be found!", "Id");
            }

            var dto = _mapper.Map<CourseDetailDto>(entity);

            return new ValidateableResponse<CourseDetailDto>(dto);
        }
    }

    public class GetCourseByIdRequestValidation : AbstractValidator<GetCourseByIdRequest>
    {
        public GetCourseByIdRequestValidation()
        {
        }
    }
}