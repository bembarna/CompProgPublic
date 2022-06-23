using AutoMapper;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Extensions;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using CompProgEdu.Core.Features.Students;
using CompProgEdu.Core.Features.Users;
using CompProgEdu.Core.Security;
using FluentValidation;
using LamarCodeGeneration.Frames;
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
    public class RegisterStudentRequest : StudentDetailDto, IRequest<ValidateableResponse<UserGetDto>>, IValidateable
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class RegisterStudentRequestHandler : IRequestHandler<RegisterStudentRequest, ValidateableResponse<UserGetDto>>
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public RegisterStudentRequestHandler(
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

        public async Task<ValidateableResponse<UserGetDto>> Handle(RegisterStudentRequest request, CancellationToken tkn)
        {
            var createStudentResponse = await _mediator.Send(new CreateStudentRequest
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                EmailAddress = request.EmailAddress,
                StudentSchoolNumber = request.StudentSchoolNumber,
            }, tkn
            );

            if (createStudentResponse.Errors.Any())
            {
                return new ValidateableResponse<UserGetDto>(createStudentResponse.Errors.ToList());
            }

            var user = _dataContext.Set<User>().FirstOrDefault(x => x.Id == createStudentResponse.Result.UserId);

            if(user == null)
            {
                return new ValidateableResponse<UserGetDto>("Unable to find student", "StudentRegistration");
            }

            await _userManager.AddPasswordAsync(user, request.Password);
            var dto = _mapper.Map<UserGetDto>(user);

            return new ValidateableResponse<UserGetDto>(dto);
        }
    }

    public class RegisterStudentRequestValidation : AbstractValidator<RegisterStudentRequest>
    {
        private readonly DataContext _dataContext;
        public RegisterStudentRequestValidation(DataContext dataContext)
        {
            _dataContext = dataContext;

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
