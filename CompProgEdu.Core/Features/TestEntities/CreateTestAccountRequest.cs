using AutoMapper;
using FluentValidation;
using MediatR;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using CompProgEdu.Core.Features.TestEntities;
using System;
using System.Threading;
using System.Threading.Tasks;
using CompProgEdu.Core.Features.Extensions;

namespace CompProgEdu.Features.TestEntities
{
    public class CreateTestAccountRequest : TestAccountDto, IRequest<ValidateableResponse<TestAccountGetDto>>, IValidateable
    {
    }

    public class CreateTestAccountRequestHandler : IRequestHandler<CreateTestAccountRequest, ValidateableResponse<TestAccountGetDto>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public CreateTestAccountRequestHandler(
            DataContext dataContext,
            IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ValidateableResponse<TestAccountGetDto>> Handle(CreateTestAccountRequest request, CancellationToken tkn)
        {
            var entity = _mapper.Map<TestAccount>(request); 

            entity.LastVisit = DateTime.UtcNow;

            await _dataContext.AddAsync(entity);
            await _dataContext.SaveChangesAsync();

            var dto = _mapper.Map<TestAccountGetDto>(entity);

            return new ValidateableResponse<TestAccountGetDto>(dto);
        }
    }

    public class CreateTestAccountRequestValidation : AbstractValidator<CreateTestAccountRequest>
    {
        private readonly DataContext _dataContext;

        public CreateTestAccountRequestValidation(DataContext dataContext)
        {
            _dataContext = dataContext;

            RuleFor(x => x.NumberOfPeople)
                .NotEmpty();

            RuleFor(x => x.EmailAddress)
                .IsUnique<CreateTestAccountRequest, TestAccount, string>(_dataContext);

            RuleFor(x => x.NumberOfPeople)
                .IsUnique<CreateTestAccountRequest, TestAccount, int>(_dataContext);
        }
    }
}