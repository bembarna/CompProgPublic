using AutoMapper;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Extensions;
using CompProgEdu.Core.Features.Instructors;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using CompProgEdu.Core.Features.Users;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CompProgEdu.Core.Features.Request
{
    public class RegisterInstructorRequest : InstructorDetailDto, IRequest<ValidateableResponse<UserGetDto>>, IValidateable
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class RegisterInstructorRequestHandler : IRequestHandler<RegisterInstructorRequest, ValidateableResponse<UserGetDto>>
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public RegisterInstructorRequestHandler(
            DataContext dataContext,
            UserManager<User> userManager,
            IMapper mapper,
            IMediator mediator)
        {
            _dataContext = dataContext;
            _userManager = userManager;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<ValidateableResponse<UserGetDto>> Handle(RegisterInstructorRequest request, CancellationToken tkn)
        {
            var createInstructorResponse = await _mediator.Send(new CreateInstructorRequest
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                EmailAddress = request.EmailAddress,
                Title = request.Title
            }, tkn
            );

            if (createInstructorResponse.Errors.Any())
            {
                return new ValidateableResponse<UserGetDto>(createInstructorResponse.Errors.ToList());
            }

            var user = _dataContext.Set<User>().FirstOrDefault(x => x.Id == createInstructorResponse.Result.UserId);

            if (user == null)
            {
                return new ValidateableResponse<UserGetDto>("Unable to find Instructor", "InstructorRegistration");
            }

            await _userManager.AddPasswordAsync(user, request.Password);
            var dto = _mapper.Map<UserGetDto>(user);

            return new ValidateableResponse<UserGetDto>(dto);
        }
    }

    public class RegisterInstructorRequestValidation : AbstractValidator<RegisterInstructorRequest>
    {
        private readonly DataContext _dataContext;
        public RegisterInstructorRequestValidation(DataContext dataContext, IValidator<UserDto> mainDtoValidator)
        {
            _dataContext = dataContext;

            RuleFor(x => x.Password)
                .Password();

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password)
                .WithMessage("Passwords do not match.");

            RuleFor(x => x.EmailAddress)
                .EmailAddress()
                .Must(EmailMyBeUnique)
                .WithMessage("Email is already registered for.");
        }

        private bool EmailMyBeUnique(string email)
        {
            return !_dataContext.Set<User>().Any(x => x.Email == email);
        }
    }
}
