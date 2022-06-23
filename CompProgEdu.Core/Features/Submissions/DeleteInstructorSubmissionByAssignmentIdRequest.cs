using AutoMapper;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Assignments;
using CompProgEdu.Core.Features.CurlySets;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CompProgEdu.Core.Features.Submissions
{
    public class DeleteInstructorSubmissionByAssignmentIdRequest : IRequest<ValidateableResponse<InstructorSubmissionDetailDto>>, IValidateable
    {
        public DeleteInstructorSubmissionByAssignmentIdRequest(int assignmentId)
        {
            AssignmentId = assignmentId;
        }

        public int AssignmentId { get; set; }
    }

    public class DeleteInstructorSubmissionByAssignmentIdRequestHandler : IRequestHandler<DeleteInstructorSubmissionByAssignmentIdRequest, ValidateableResponse<InstructorSubmissionDetailDto>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public DeleteInstructorSubmissionByAssignmentIdRequestHandler(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ValidateableResponse<InstructorSubmissionDetailDto>> Handle(DeleteInstructorSubmissionByAssignmentIdRequest request, CancellationToken tkn)
        {
            var assignment = await _dataContext.Set<Assignment>().FirstAsync(x => x.Id == request.AssignmentId, tkn);
            var instructorSubmission = _dataContext.Set<InstructorSubmission>()
                .First(x => x.AssignmentId == request.AssignmentId);
            var methodSignature = await _dataContext.Set<MethodSignature>()
                .Where(x => x.AssignmentId == request.AssignmentId)
                .Include(x => x.MethodParameters)
                .FirstOrDefaultAsync(tkn);
            var classSignatures = await _dataContext.Set<ClassSignature>()
                .Where(x => x.AssignmentId == request.AssignmentId).ToListAsync(tkn);
            var curlySets = await _dataContext.Set<CurlySet>().Where(x => x.AssignmentId == request.AssignmentId)
                .ToListAsync(tkn);
            var primitiveStatements = await _dataContext.Set<PrimitiveStatement>()
                .Where(x => x.AssignmentId == request.AssignmentId)
                .ToListAsync(tkn);
            var primitiveVariables = await _dataContext.Set<PrimitiveVariable>()
                .Where(x => x.AssignmentId == request.AssignmentId)
                .ToListAsync(tkn);
            var properties= await _dataContext.Set<PropertySignature>()
                .Where(x => x.AssignmentId == request.AssignmentId)
                .ToListAsync(tkn);

            assignment.AssignmentSolutionFileName = null;
            _dataContext.Update(assignment);
            _dataContext?.Remove(instructorSubmission);
            _dataContext?.Remove(methodSignature);
            _dataContext.RemoveRange(classSignatures);
            _dataContext.RemoveRange(curlySets);
            _dataContext.RemoveRange(primitiveStatements);
            _dataContext.RemoveRange(primitiveVariables);
            _dataContext.RemoveRange(properties);

            await _dataContext.SaveChangesAsync(tkn);

            var dto = _mapper.Map<InstructorSubmissionDetailDto>(instructorSubmission);

            return new ValidateableResponse<InstructorSubmissionDetailDto>(dto);
        }

        public class DeleteInstructorSubmissionByAssignmentIdRequestValidation : AbstractValidator<DeleteInstructorSubmissionByAssignmentIdRequest>
        {
            private readonly DataContext _dataContext;

            public DeleteInstructorSubmissionByAssignmentIdRequestValidation(DataContext dataContext)
            {
                _dataContext = dataContext;
            }
        }
    }
}
