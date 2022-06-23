using AutoMapper;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Extensions;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace CompProgEdu.Core.Features.TestEntities
{
    public class GetTestAccountByIdRequest : IRequest<ValidateableResponse<TestAccountGetDto>>, IValidateable
    {
        public int Id { get; set; }
    }

    public class GetTestAccountByIdRequestHandler : IRequestHandler<GetTestAccountByIdRequest, ValidateableResponse<TestAccountGetDto>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public GetTestAccountByIdRequestHandler(
            DataContext dataContext,
            IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ValidateableResponse<TestAccountGetDto>> Handle(GetTestAccountByIdRequest request, CancellationToken tkn)
        {
            var entity = await _dataContext.Set<TestAccount>().FirstOrDefaultAsync(x => x.Id == request.Id, tkn);

            if (entity == null)
            {
                return new ValidateableResponse<TestAccountGetDto>($"Test Account with Id of {request.Id} could not be found!", "Id");//TODO ADD THIS TO AN RuleFor THIS IS JUST TEMP TO SHOW HOW INLINE ERROR HANDLING WORKS
            }

            var dto = _mapper.Map<TestAccountGetDto>(entity);

            return new ValidateableResponse<TestAccountGetDto>(dto);
        }
    }

    public class GetTestAccountByIdRequestValidation : AbstractValidator<GetTestAccountByIdRequest>
    {
        private readonly DataContext _dataContext;

        public GetTestAccountByIdRequestValidation(DataContext dataContext)
        {
            _dataContext = dataContext;

            RuleFor(x => x.Id)
                .EntityMustExist<GetTestAccountByIdRequest, TestAccount>(_dataContext);
        }
    }
}
