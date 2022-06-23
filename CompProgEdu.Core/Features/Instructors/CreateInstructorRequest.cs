using AutoMapper;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using CompProgEdu.Core.Features.Users;
using CompProgEdu.Core.Security;
using FluentValidation;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CompProgEdu.Core.Features.Instructors
{
    public class CreateInstructorRequest : InstructorDetailDto, IRequest<ValidateableResponse<InstructorResponseDto>>, IValidateable
    {
    }

    public class CreateInstructorRequestHandler : IRequestHandler<CreateInstructorRequest, ValidateableResponse<InstructorResponseDto>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public CreateInstructorRequestHandler(
            DataContext dataContext,
            IMapper mapper,
            IMediator mediator)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<ValidateableResponse<InstructorResponseDto>> Handle(CreateInstructorRequest request, CancellationToken tkn)
        {
            var entity = new Instructor
            {
                Title = request.Title,
            };

            var createUserResponse = await _mediator.Send(new CreateUserRequest
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                EmailAddress = request.EmailAddress,
                Role = Roles.Instructor
            }, tkn
            );

            if (createUserResponse.Errors.Any())
            {
                return new ValidateableResponse<InstructorResponseDto>(createUserResponse.Errors.ToList());
            }

            entity.UserId = createUserResponse.Result.Id;           
            await _dataContext.AddAsync(entity);
            await _dataContext.SaveChangesAsync();

            var dto = _mapper.Map<InstructorResponseDto>(entity);

            return new ValidateableResponse<InstructorResponseDto>(dto);
        }
    }

    public class CreateInstructorRequestValidation : AbstractValidator<CreateInstructorRequest>
    {
        private readonly DataContext _dataContext;
        public CreateInstructorRequestValidation(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
    }
}
