using AutoMapper;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using FluentValidation;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CompProgEdu.Core.Features.Extensions;

namespace CompProgEdu.Core.Features.TestEntities
{
    public class DeleteTestAccountByIdRequest : IRequest<ValidateableResponse<TestAccountGetDto>>, IValidateable
    {
        public int Id { get; set; }
    }

    public class DeleteTestAccountByIdRequestHandler : IRequestHandler<DeleteTestAccountByIdRequest, ValidateableResponse<TestAccountGetDto>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public DeleteTestAccountByIdRequestHandler(
            DataContext dataContext,
            IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ValidateableResponse<TestAccountGetDto>> Handle(DeleteTestAccountByIdRequest request, CancellationToken tkn)
        {
            var entity = _dataContext.Set<TestAccount>().FirstOrDefault(x => x.Id == request.Id);

            if (entity == null)
            {
                return new ValidateableResponse<TestAccountGetDto>($"Test Account with Id of {request.Id} could not be found!", "Id");//TODO ADD THIS TO AN RuleFor THIS IS JUST TEMP TO SHOW HOW INLINE ERROR HANDLING WORKS
            }

            var dto = _mapper.Map<TestAccountGetDto>(entity);

            _dataContext.Remove(entity);
            await _dataContext.SaveChangesAsync();

            return new ValidateableResponse<TestAccountGetDto>(dto);
        }
    }

    public class DeleteTestAccountByIdRequestValidation : AbstractValidator<DeleteTestAccountByIdRequest>
    {
        private readonly DataContext _dataContext;

        public DeleteTestAccountByIdRequestValidation(DataContext dataContext)
        {
            _dataContext = dataContext;

            RuleFor(x => x.Id)
                .EntityMustExist<DeleteTestAccountByIdRequest, TestAccount>(_dataContext);
        }
    }
}
