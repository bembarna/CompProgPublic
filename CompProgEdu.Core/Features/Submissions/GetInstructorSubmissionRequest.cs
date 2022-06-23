using AutoMapper;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.CurlySets;
using CompProgEdu.Core.Features.Extensions;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CompProgEdu.Core.Features.Submissions
{
    public class GetInstructorSubmissionRequest : IRequest<ValidateableResponse<InstructorSubmissionDetailDto>>, IValidateable
    {
        public int Id { get; set; }
    }

    public class GetInstructorSubmissionRequestHandler : IRequestHandler<GetInstructorSubmissionRequest, ValidateableResponse<InstructorSubmissionDetailDto>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public GetInstructorSubmissionRequestHandler(
            DataContext dataContext,
            IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ValidateableResponse<InstructorSubmissionDetailDto>> Handle(GetInstructorSubmissionRequest request, CancellationToken tkn)
        {
            var entity = await _dataContext.Set<InstructorSubmission>()
                .Where(x => x.Id == request.Id)
                .Include(x => x.CurlySet)
                .ThenInclude(x => x.ClassSignature)
                .Include(x => x.CurlySet)
                .ThenInclude(x => x.MethodSignature)
                .Include(x => x.CurlySet)
                .ThenInclude(x => x.StatementSignature)
                .Include(x => x.CurlySet)
                .ThenInclude(x => x.PrimitiveVariables)
                .Include(x => x.CurlySet)
                .ThenInclude(x => x.CurlySets)
                .FirstOrDefaultAsync(tkn);

            var dto = _mapper.Map<InstructorSubmissionDetailDto>(entity);

            return new ValidateableResponse<InstructorSubmissionDetailDto>(dto);
        }
    }

    public class GetInstructorSubmissionRequestValidation : AbstractValidator<GetInstructorSubmissionRequest>
    {
        private readonly DataContext _dataContext;

        public GetInstructorSubmissionRequestValidation(DataContext dataContext)
        {
            _dataContext = dataContext;

            RuleFor(x => x.Id)
                .EntityMustExist<GetInstructorSubmissionRequest, InstructorSubmission>(_dataContext);
        }
    }
}