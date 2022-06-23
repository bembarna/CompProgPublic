using AutoMapper;
using FluentValidation;
using MediatR;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using CompProgEdu.Core.Features.Extensions;

namespace CompProgEdu.Core.Features.TestEntities
{
    public class UpdateTestAccountByIdRequest : TestAccountDto, IRequest<ValidateableResponse<TestAccountGetDto>>, IValidateable
    {
        public int Id { get; set; }
    }

    public class UpdateTestAccountByIdRequestHandler : IRequestHandler<UpdateTestAccountByIdRequest, ValidateableResponse<TestAccountGetDto>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public UpdateTestAccountByIdRequestHandler(
            DataContext dataContext,
            IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ValidateableResponse<TestAccountGetDto>> Handle(UpdateTestAccountByIdRequest request, CancellationToken tkn)
        {
            var entity = _dataContext.Set<TestAccount>().FirstOrDefault(x => x.Id == request.Id);

            var errorList = new List<ErrorResponse>();//THIS IS WHAT IT LOOKS LIKE FOR MULTIPLE INLINE ERRORS, THIS WILL MAINLY JUST BE USED WHEN CHECKING AND SENDING BACK MULTIPLE TESTCASE ERRORS FOR ASSIGNMENTS

            if (entity == null)
            {
                return new ValidateableResponse<TestAccountGetDto>($"Test Account with Id of {request.Id} could not be found!", "Id");//THIS IS WHAT IT LOOKS LIKE FOR A SINGLE INLINE ERROR
            }

            _mapper.Map(request, entity);

            if(entity.AccountName == null)//THIS IS WHAT IT LOOKS LIKE FOR MULTIPLE INLINE ERRORS, THIS WILL MAINLY JUST BE USED WHEN CHECKING AND SENDING BACK MULTIPLE TESTCASE ERRORS FOR ASSIGNMENTS
            {
                errorList.Add(new ErrorResponse { FieldName = "AccountName", Error = "AccountName must not be null" });
            }

            if (entity.AccountNumber == null)//THIS IS WHAT IT LOOKS LIKE FOR MULTIPLE INLINE ERRORS, THIS WILL MAINLY JUST BE USED WHEN CHECKING AND SENDING BACK MULTIPLE TESTCASE ERRORS FOR ASSIGNMENTS
            {
                errorList.Add(new ErrorResponse { FieldName = "AccountNumber", Error = "AccountNumber must not be null" });
            }

            if (errorList.Any())//THIS IS WHAT IT LOOKS LIKE FOR MULTIPLE INLINE ERRORS, THIS WILL MAINLY JUST BE USED WHEN CHECKING AND SENDING BACK MULTIPLE TESTCASE ERRORS FOR ASSIGNMENTS
            {
                return new ValidateableResponse<TestAccountGetDto>(errorList);
            }

            _dataContext.Update(entity);
            await _dataContext.SaveChangesAsync();

            var dto = _mapper.Map<TestAccountGetDto>(entity);

            return new ValidateableResponse<TestAccountGetDto>(dto);
        }

    }

    public class UpdateTestAccountByIdRequestValidation : AbstractValidator<UpdateTestAccountByIdRequest>
    {
        private readonly DataContext _dataContext;
        public UpdateTestAccountByIdRequestValidation(DataContext dataContext)
        {
            _dataContext = dataContext;

            RuleFor(x => x.NumberOfPeople)
                .LessThanOrEqualTo(10);

            RuleFor(x => x.Id)
                .EntityMustExist<UpdateTestAccountByIdRequest, TestAccount>(_dataContext);

            RuleFor(x => x.EmailAddress)
                .IsUnique<UpdateTestAccountByIdRequest, TestAccount, string>(_dataContext);

            RuleFor(x => x.NumberOfPeople)
                .IsUnique<UpdateTestAccountByIdRequest, TestAccount, int>(_dataContext);
        }
    }
}
