using AutoMapper;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Extensions;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using CompProgEdu.Core.Features.Users;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CompProgEdu.Core.Features.Students
{
    public class UpdateStudentPasswordRequest : IRequest<ValidateableResponse<StudentResponseDto>>, IValidateable
    {
        public int Id { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class UpdateStudentPasswordRequestHandler : IRequestHandler<UpdateStudentPasswordRequest, ValidateableResponse<StudentResponseDto>>
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UpdateStudentPasswordRequestHandler(
            DataContext dataContext,
            UserManager<User> userManager,
            IMapper mapper)
        {
            _dataContext = dataContext;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<ValidateableResponse<StudentResponseDto>> Handle(UpdateStudentPasswordRequest request, CancellationToken cancellationToken)
        {
            var student = _dataContext.Set<Student>()
                .Where(x => x.Id == request.Id)
                .Include(x => x.User)
                .First();

            if (student.ChangedPassword)
            {
                return new ValidateableResponse<StudentResponseDto>("This student has already changed their password since their first sign up. In order to change again, they must do it through the UpdateUserPasswordRequest.", "");
            }

            await _userManager.RemovePasswordAsync(student.User);
            await _userManager.AddPasswordAsync(student.User, request.NewPassword);
            await _userManager.UpdateAsync(student.User);

            student.ChangedPassword = true;
            _dataContext.Update(student);
            await _dataContext.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<StudentResponseDto>(student);
            return new ValidateableResponse<StudentResponseDto>(response);
        }
    }

    public class UpdateStudentPasswordRequestValidator : AbstractValidator<UpdateStudentPasswordRequest>
    {
        private readonly DataContext _dataContext;

        public UpdateStudentPasswordRequestValidator(
            DataContext dataContext)
        {
            _dataContext = dataContext;

            RuleFor(x => x.NewPassword)
                .Equal(x => x.ConfirmPassword)
                .WithMessage("Passwords do not match.");

            RuleFor(x => x.NewPassword)
                .Password();
        }
    }
}
