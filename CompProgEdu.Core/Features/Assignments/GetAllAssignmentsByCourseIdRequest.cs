using AutoMapper;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Courses;
using CompProgEdu.Core.Features.Extensions;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CompProgEdu.Core.Features.Assignments
{
    public class GetAllAssignmentsByCourseIdRequest : IRequest<ValidateableResponse<List<AssignmentSummaryDto>>>, IValidateable
    {
        public int CourseId { get; set; }
    }

    public class GetAllAssignmentsByCourseIdRequestHandler : IRequestHandler<GetAllAssignmentsByCourseIdRequest, ValidateableResponse<List<AssignmentSummaryDto>>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public GetAllAssignmentsByCourseIdRequestHandler(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ValidateableResponse<List<AssignmentSummaryDto>>> Handle(GetAllAssignmentsByCourseIdRequest request, CancellationToken cancellationToken)
        {
            var assignments = await _dataContext.Set<Assignment>()
                .Include(x => x.Course)
                .Where(x => x.CourseId == request.CourseId)
                .ToListAsync();

            var dtos = _mapper.Map<List<AssignmentSummaryDto>>(assignments);

            return new ValidateableResponse<List<AssignmentSummaryDto>>(dtos);
        }
    }

    public class GetAllAssignmentsByCourseIdRequestValidation : AbstractValidator<GetAllAssignmentsByCourseIdRequest>
    {
        private DataContext _dataContext;

        public GetAllAssignmentsByCourseIdRequestValidation(DataContext dataContext)
        {
            _dataContext = dataContext;

            RuleFor(x => x.CourseId)
                .EntityMustExist<GetAllAssignmentsByCourseIdRequest, Course>(_dataContext);
        }
    }
}
