using AutoMapper;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CompProgEdu.Core.Features.TestCases
{
    public class GetMethodTestCaseByIdRequest : IRequest<ValidateableResponse<MethodTestCaseGetDto>>, IValidateable
    {
        public GetMethodTestCaseByIdRequest(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }

    public class GetMethodTestCaseByIdRequestHandler : IRequestHandler<GetMethodTestCaseByIdRequest, ValidateableResponse<MethodTestCaseGetDto>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public GetMethodTestCaseByIdRequestHandler(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ValidateableResponse<MethodTestCaseGetDto>> Handle(GetMethodTestCaseByIdRequest request, CancellationToken tkn)
        {
            var desiredOutput = _dataContext.Set<MethodTestCase>().First(x => x.Id == request.Id);
            var dto = _mapper.Map<MethodTestCaseGetDto>(desiredOutput);
            return new ValidateableResponse<MethodTestCaseGetDto>(dto);
        }
    }

    public class GetMethodTestCaseByIdRequestValidation : AbstractValidator<GetMethodTestCaseByIdRequest>
    {
        private readonly DataContext _dataContext;
        public GetMethodTestCaseByIdRequestValidation(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
    }
}

