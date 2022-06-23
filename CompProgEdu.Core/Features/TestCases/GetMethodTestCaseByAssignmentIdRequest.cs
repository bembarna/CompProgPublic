using AutoMapper;
using CompProgEdu.Core.Data;
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

namespace CompProgEdu.Core.Features.TestCases
{
    public class GetMethodTestCaseByAssignmentIdRequest : IRequest<ValidateableResponse<List<MethodTestCaseGetDto>>>, IValidateable
    {
        public GetMethodTestCaseByAssignmentIdRequest(int assignmentId)
        {
            AssignmentId = assignmentId;
        }

        public int AssignmentId { get; set; }
    }

    public class GetMethodTestCaseByAssignmentIdRequestHandler : IRequestHandler<GetMethodTestCaseByAssignmentIdRequest, ValidateableResponse<List<MethodTestCaseGetDto>>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public GetMethodTestCaseByAssignmentIdRequestHandler(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ValidateableResponse<List<MethodTestCaseGetDto>>> Handle(GetMethodTestCaseByAssignmentIdRequest request, CancellationToken tkn)
        {
            var desiredOutput = await _dataContext.Set<MethodTestCase>()
                .Where(x => x.AssignmentId == request.AssignmentId)
                .ToListAsync(tkn);
            var dto = _mapper.Map<List<MethodTestCaseGetDto>>(desiredOutput);
            return new ValidateableResponse<List<MethodTestCaseGetDto>>(dto);
        }
    }

    public class GetMethodTestCaseByAssignmentIdRequestValidation : AbstractValidator<GetMethodTestCaseByAssignmentIdRequest>
    {
        private readonly DataContext _dataContext;
        public GetMethodTestCaseByAssignmentIdRequestValidation(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
    }
}
