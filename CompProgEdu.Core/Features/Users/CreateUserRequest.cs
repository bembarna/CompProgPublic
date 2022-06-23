using AutoMapper;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Extensions;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CompProgEdu.Core.Features.Users
{
    public class CreateUserRequest : UserDto, IRequest<ValidateableResponse<UserGetDto>>, IValidateable
    {
    }

    public class CreateUserRequestHandler : IRequestHandler<CreateUserRequest, ValidateableResponse<UserGetDto>>
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public CreateUserRequestHandler(
            DataContext dataContext,
            UserManager<User> userManager,
            IMapper mapper)
        {
            _dataContext = dataContext;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<ValidateableResponse<UserGetDto>> Handle(CreateUserRequest request, CancellationToken tkn)
        {
            var entity = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.EmailAddress,
                UserName = request.EmailAddress,
                EmailConfirmed = true//TEMP REMOVE LATER
            };

            entity.SecurityStamp = await _userManager.GenerateConcurrencyStampAsync(entity);
            var createUserResult = await _userManager.CreateAsync(entity);

            if (createUserResult.Errors.Any())
            {
                return new ValidateableResponse<UserGetDto>("Email Address already in use!", "Invalid Signup");
            }

            await _userManager.AddToRoleAsync(entity, request.Role);
            var dto = _mapper.Map<UserGetDto>(entity);

            return new ValidateableResponse<UserGetDto>(dto);
        }
    }

    public class CreateUserRequestValidation : AbstractValidator<CreateUserRequest>
    {
        private readonly DataContext _dataContext;
        public CreateUserRequestValidation(DataContext dataContext, IValidator<UserDto> mainDtoValidator)
        {
            _dataContext = dataContext;

            Include(mainDtoValidator);

            RuleFor(x => x.EmailAddress)//THIS MUST STAY AS IsUniqueWithDbList for Users, WILL EXPLAIN WHY IF YOU NEED ME TO - Aidan
                .IsUniqueWithDbList(
                    _dataContext
                    .Set<User>()
                    .Select(x => x.Email)
                    .ToList()
                );
        }
    }
}
