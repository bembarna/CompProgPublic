using AutoMapper;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Courses;
using CompProgEdu.Core.Features.Extensions;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CompProgEdu.Core.Features.Assignments
{
    public class GetAllStudentAssignmentsByCourseIdRequest : IRequest<ValidateableResponse<List<AssignmentSummaryDto>>>, IValidateable
    {
        public int CourseId { get; set; }
    }

    public class GetAllStudentAssignmentsByCourseIdRequestHandler : IRequestHandler<GetAllStudentAssignmentsByCourseIdRequest, ValidateableResponse<List<AssignmentSummaryDto>>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public GetAllStudentAssignmentsByCourseIdRequestHandler(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ValidateableResponse<List<AssignmentSummaryDto>>> Handle(GetAllStudentAssignmentsByCourseIdRequest request, CancellationToken cancellationToken)
        {
            var assignments = await _dataContext.Set<Assignment>()
                .Include(x => x.Course)
                .Where(x => x.CourseId == request.CourseId)
                .Where(x => x.VisibilityDate <= DateTime.UtcNow)
                .ToListAsync(cancellationToken);

            var dtos = _mapper.Map<List<AssignmentSummaryDto>>(assignments);

            return new ValidateableResponse<List<AssignmentSummaryDto>>(dtos);
        }
    }

    public class GetAllStudentAssignmentsByCourseIdRequestValidation : AbstractValidator<GetAllStudentAssignmentsByCourseIdRequest>
    {
        private DataContext _dataContext;

        public GetAllStudentAssignmentsByCourseIdRequestValidation(DataContext dataContext)
        {
            _dataContext = dataContext;

            RuleFor(x => x.CourseId)
                .EntityMustExist<GetAllStudentAssignmentsByCourseIdRequest, Course>(_dataContext);
        }
    }
}
