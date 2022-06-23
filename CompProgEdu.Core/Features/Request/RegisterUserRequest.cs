using AutoMapper;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Extensions;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using CompProgEdu.Core.Features.Users;
using CompProgEdu.Core.Security;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CompProgEdu.Core.Features.Request
{
    public class RegisterRequest : UserDto, IRequest<ValidateableResponse<UserGetDto>>, IValidateable
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class RegisterRequestHandler : IRequestHandler<RegisterRequest, ValidateableResponse<UserGetDto>>
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public RegisterRequestHandler(
            DataContext dataContext,
            UserManager<User> userManager,
            IMapper mapper)
        {
            _dataContext = dataContext;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<ValidateableResponse<UserGetDto>> Handle(RegisterRequest request, CancellationToken tkn)
        {
            var entity = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.EmailAddress,
                UserName = request.EmailAddress,
                EmailConfirmed = true//TEMP REMOVE LATER
            };

            request.Role = Roles.GlobalAdmin;//temp remove later

            entity.SecurityStamp = await _userManager.GenerateConcurrencyStampAsync(entity);
            await _userManager.CreateAsync(entity, request.Password);
            await _userManager.AddToRoleAsync(entity, request.Role);
            var dto = _mapper.Map<UserGetDto>(entity);

            return new ValidateableResponse<UserGetDto>(dto);
        }
    }

    public class RegisterRequestValidation : AbstractValidator<RegisterRequest>
    {
        private readonly DataContext _dataContext;
        public RegisterRequestValidation(DataContext dataContext, IValidator<UserDto> mainDtoValidator)
        {
            _dataContext = dataContext;

            Include(mainDtoValidator);

            RuleFor(x => x.Password)
                .Password();

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password)
                .WithMessage("Passwords do not match.");

            RuleFor(x => x.EmailAddress)
                .Must(EmailMyBeUnique)
                .WithMessage("Email is already registered for.");
        }

        private bool EmailMyBeUnique(string email)
        {
            return !_dataContext.Set<User>().Any(x => x.Email == email);
        }
    }

}
