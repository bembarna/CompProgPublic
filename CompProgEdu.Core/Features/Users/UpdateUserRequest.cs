using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CompProgEdu.Core.Features.Extensions;

namespace CompProgEdu.Core.Features.Users
{
    public class UpdateUserRequest : UserDto, IRequest<ValidateableResponse<UserGetDto>>, IValidateable
    {
        public int Id { get; set; }
    }

    public class UpdateUserRequestHandler : IRequestHandler<UpdateUserRequest, ValidateableResponse<UserGetDto>>
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UpdateUserRequestHandler(
            DataContext dataContext,
            UserManager<User> userManager,
            IMapper mapper)
        {
            _dataContext = dataContext;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<ValidateableResponse<UserGetDto>> Handle(UpdateUserRequest request, CancellationToken tkn)
        {
            var entity = _dataContext.Set<User>().Include(x => x.UserRoles).ThenInclude(x => x.Role).FirstOrDefault(x => x.Id == request.Id);

            entity.FirstName = request.FirstName;
            entity.LastName = request.LastName;
            entity.Email = request.EmailAddress;
            entity.UserName = request.EmailAddress;

            if (entity.UserRoles.Any())
            {
                await _userManager.RemoveFromRoleAsync(entity, entity.UserRoles.First().Role.Name);
            }

            var test = await _userManager.AddToRoleAsync(entity, request.Role);

            var updateResult = await _userManager.UpdateAsync(entity);

            if (updateResult.Errors.Any())
            {
                return new ValidateableResponse<UserGetDto>("Email Address already in use!", "Invalid Signup");
            }

            var dto = _mapper.Map<UserGetDto>(entity);

            return new ValidateableResponse<UserGetDto>(dto);
        }
    }

    public class UpdateUserRequestValidation : AbstractValidator<UpdateUserRequest>
    {
        private readonly DataContext _dataContext;
        public UpdateUserRequestValidation(DataContext dataContext, IValidator<UserDto> mainDtoValidator)
        {
            _dataContext = dataContext;

            Include(mainDtoValidator);

            RuleFor(x => x.Id)
                .EntityMustExist<UpdateUserRequest, User>(_dataContext);

            RuleFor(x => x.EmailAddress)
                .Must(EmailMyBeUnique)
                .WithMessage("Email must be unique.");
        }

        private bool EmailMyBeUnique(UpdateUserRequest request, string email)
        {
            return !_dataContext.Set<User>().Any(x => x.Id != request.Id && x.Email == email);
        }   
    }
}
