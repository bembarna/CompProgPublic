using AutoMapper;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.CurlySets;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CompProgEdu.Core.Features.TestCases
{
    public class CreateMethodTestCaseRequest : IRequest<ValidateableResponse<MethodTestCaseGetDto>>, IValidateable
    {
        public MethodTestCaseDto MethodTestCaseDto { get; set; }
        public string MethodNodeKey { get; set; }//class-1538/method-1546
    }

    public class CreateMethodTestCaseRequestHandler : IRequestHandler<CreateMethodTestCaseRequest, ValidateableResponse<MethodTestCaseGetDto>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public CreateMethodTestCaseRequestHandler(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ValidateableResponse<MethodTestCaseGetDto>> Handle(CreateMethodTestCaseRequest request, CancellationToken tkn)
        {
            var nodeKeyName = GetNodeName(request.MethodNodeKey);
            if(nodeKeyName == "method")
            {
                var methodSignatureId = GetMethodSignatureId(request.MethodNodeKey);
                var methodSignature = _dataContext.Set<CurlySet>().Include(x => x.MethodSignature).FirstOrDefault(x => x.Id == methodSignatureId).MethodSignature;
                if(methodSignature == null)
                {
                    return new ValidateableResponse<MethodTestCaseGetDto>($"MethodSignature with Id of {methodSignatureId} could not be found!", "Id");
                }
                var methodInjectable = BuildMethodCall(methodSignature.MethodName, request.MethodTestCaseDto.ParamInputs);
                var methodTestCase = _mapper.Map<MethodTestCase>(request.MethodTestCaseDto);
                methodTestCase.MethodTestInjectable = methodInjectable;
                methodTestCase.ReturnType = methodSignature.ReturnType.Trim();
                await _dataContext.AddAsync(methodTestCase, tkn);
                await _dataContext.SaveChangesAsync(tkn);
                methodSignature.MethodTestCaseId = methodTestCase.Id;
                await _dataContext.SaveChangesAsync(tkn);
                var dto = _mapper.Map<MethodTestCaseGetDto>(methodTestCase);
                return new ValidateableResponse<MethodTestCaseGetDto>(dto);
            }

            return new ValidateableResponse<MethodTestCaseGetDto>($"Method Name invalid", "MethodName");
        }
        public string BuildMethodCall(string methodName, string methodInputs)
        {
            return methodName + "(" + methodInputs + ")";
        }
        public int GetMethodSignatureId(string methodNodeKey)
        {
            var methodSignatureId = Convert.ToInt32(methodNodeKey.Split('/').Last().Trim().Split('-').Last().Trim());
            return methodSignatureId;
        }

        public string GetNodeName(string methodNodeKey)
        {
            var nodeName = methodNodeKey.Split('/').Last().Trim().Split('-').First().Trim();
            return nodeName;
        }
    }

    public class CreateMethodTestCaseRequestValidation : AbstractValidator<CreateMethodTestCaseRequest>
    {
        public CreateMethodTestCaseRequestValidation(IValidator<MethodTestCaseDto> mainDtoValidator)
        {
            RuleFor(x => x.MethodTestCaseDto.ParamInputs)
                .NotEmpty();

            RuleFor(x => x.MethodTestCaseDto.Output)
                .NotEmpty();

            RuleFor(x => x.MethodTestCaseDto.PointValue)
                .NotNull();
        }
    }
}


