using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CompProgEdu.Core.Features.Users
{
    public class UpdateUserPasswordRequest : IRequest<ValidateableResponse<UserGetDto>>, IValidateable
    {
        public int Id { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class UpdateUserPasswordRequestHandler : IRequestHandler<UpdateUserPasswordRequest, ValidateableResponse<UserGetDto>>
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<User> _userManager;

        public UpdateUserPasswordRequestHandler(
            DataContext dataContext,
            UserManager<User> userManager)
        {
            _dataContext = dataContext;
            _userManager = userManager;
        }

        public Task<ValidateableResponse<UserGetDto>> Handle(UpdateUserPasswordRequest request, CancellationToken cancellationToken)
        {
            //TODO: Finish this request lol.

            throw new NotImplementedException();
        }
    }

    public class UpdateUserPasswordRequestValidator : AbstractValidator<UpdateUserPasswordRequest>
    {
        private readonly DataContext _dataContext;

        public UpdateUserPasswordRequestValidator(
            DataContext dataContext)
        {
            _dataContext = dataContext;
        }
    }
}