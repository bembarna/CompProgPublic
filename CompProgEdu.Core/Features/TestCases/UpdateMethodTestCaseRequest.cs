using AutoMapper;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.CurlySets;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace CompProgEdu.Core.Features.TestCases
{
    public class UpdateMethodTestCaseRequest : MethodTestCaseGetDto, IRequest<ValidateableResponse<MethodTestCaseGetDto>>, IValidateable
    {
    }

    public class UpdateMethodTestCaseRequestHandler : IRequestHandler<UpdateMethodTestCaseRequest, ValidateableResponse<MethodTestCaseGetDto>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public UpdateMethodTestCaseRequestHandler(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ValidateableResponse<MethodTestCaseGetDto>> Handle(UpdateMethodTestCaseRequest request, CancellationToken tkn)
        {
            var methodTest = await _dataContext.Set<MethodTestCase>().FirstOrDefaultAsync(x => x.Id == request.Id, tkn);
            var methodSignature = await _dataContext.Set<MethodSignature>().FirstOrDefaultAsync(x => x.MethodTestCaseId == request.Id);

            methodTest.ParamInputs = request.ParamInputs;
            methodTest.Output = request.Output;
            methodTest.PointValue = request.PointValue;
            methodTest.Hint = request.Hint;
            methodTest.MethodTestInjectable = RebuildMethodCall(methodSignature.MethodName, request.ParamInputs);

            _dataContext.Update(methodTest);
            await _dataContext.SaveChangesAsync(tkn);

            var dto = _mapper.Map<MethodTestCaseGetDto>(methodTest);

            return new ValidateableResponse<MethodTestCaseGetDto>(dto);
        }

        private string RebuildMethodCall(string methodName, string paramInputs)
        {
            return methodName + "(" + paramInputs + ")";
        }
    }

    public class UpdateMethodTestCaseRequestValidation : AbstractValidator<UpdateMethodTestCaseRequest>
    {
        private readonly DataContext _dataContext;

        public UpdateMethodTestCaseRequestValidation(DataContext dataContext)
        {
            RuleFor(x => x.ParamInputs)
                .NotEmpty();

            RuleFor(x => x.Output)
                .NotEmpty();

            RuleFor(x => x.PointValue)
                .NotNull();
        }
    }
}

    

