using AutoMapper;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CompProgEdu.Core.Features.DesiredOutputs
{
    public class GetDesiredOutputsByAssignmentIdRequest : IRequest<ValidateableResponse<List<DesiredOutputGetDto>>>, IValidateable
    {
        public GetDesiredOutputsByAssignmentIdRequest(int assignmentId)
        {
            AssignmentId = assignmentId;
        }

        public int AssignmentId { get; set; }
    }

    public class GetDesiredOutputsByAssignmentIdRequestHandler : IRequestHandler<GetDesiredOutputsByAssignmentIdRequest, ValidateableResponse<List<DesiredOutputGetDto>>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public GetDesiredOutputsByAssignmentIdRequestHandler(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ValidateableResponse<List<DesiredOutputGetDto>>> Handle(GetDesiredOutputsByAssignmentIdRequest request, CancellationToken tkn)
        {
            var desiredOutput = await _dataContext.Set<DesiredOutput>()
                .Where(x => x.AssignmentId == request.AssignmentId)
                .OrderBy(x => x.Order)
                .ToListAsync(tkn);
            var dto = _mapper.Map<List<DesiredOutputGetDto>>(desiredOutput);
            return new ValidateableResponse<List<DesiredOutputGetDto>>(dto);
        }
    }

    public class GetDesiredOutputsByAssignmentIdRequestValidation : AbstractValidator<GetDesiredOutputsByAssignmentIdRequest>
    {
        private readonly DataContext _dataContext;
        public GetDesiredOutputsByAssignmentIdRequestValidation(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
    }
}