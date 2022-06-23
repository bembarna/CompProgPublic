using AutoMapper;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CompProgEdu.Core.Features.TestEntities
{
    public class GetAllTestAccountsRequest : IRequest<ValidateableResponse<List<TestAccountGetDto>>>, IValidateable
    {
    }

    public class GetAllTestAccountsRequestHandler : IRequestHandler<GetAllTestAccountsRequest, ValidateableResponse<List<TestAccountGetDto>>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public GetAllTestAccountsRequestHandler(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ValidateableResponse<List<TestAccountGetDto>>> Handle(GetAllTestAccountsRequest request, CancellationToken cancellationToken)
        {
            var testAccounts = await _dataContext.Set<TestAccount>().ToListAsync(cancellationToken);

            var dtos = _mapper.Map<List<TestAccountGetDto>>(testAccounts);

            return new ValidateableResponse<List<TestAccountGetDto>>(dtos);
        }
    }

    public class GetAllTestAccountValidation : AbstractValidator<GetAllTestAccountsRequest>
    {
        public GetAllTestAccountValidation()
        {

        }
    }
}
