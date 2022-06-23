using AutoMapper;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using CompProgEdu.Core.Features.Users;
using CompProgEdu.Core.Security;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PasswordGenerator;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace CompProgEdu.Core.Features.Students
{
    public class CreateStudentRequest : StudentDetailDto, IRequest<ValidateableResponse<StudentResponseDto>>, IValidateable
    {
    }

    public class CreateStudentRequestHandler : IRequestHandler<CreateStudentRequest, ValidateableResponse<StudentResponseDto>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly UserManager<User> _userManager;

        public CreateStudentRequestHandler(
            DataContext dataContext,
            IMapper mapper,
            IMediator mediator,
            UserManager<User> userManager)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _mediator = mediator;
            _userManager = userManager;
        }

        public async Task<ValidateableResponse<StudentResponseDto>> Handle(CreateStudentRequest request, CancellationToken tkn)
        {
            var entity = new Student
            {
                Grade = 0,
                StudentSchoolNumber = request.StudentSchoolNumber
            };

            var createUserResponse = await _mediator.Send(new CreateUserRequest
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                EmailAddress = request.EmailAddress,
                Role = Roles.Student
            }, tkn
            );

            if (createUserResponse.Errors.Any())
            {
                return new ValidateableResponse<StudentResponseDto>(createUserResponse.Errors.ToList());
            }

            var user = _dataContext.Set<User>().First(x => x.Id == createUserResponse.Result.Id);

            if (!_userManager.HasPasswordAsync(user).Result)
            {
                var password = new Password().Next();
                await _userManager.AddPasswordAsync(user, password);

                entity.UserId = createUserResponse.Result.Id;
                await _dataContext.AddAsync(entity, tkn);
                await _dataContext.SaveChangesAsync(tkn);

                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("compprogtest@gmail.com", "BigBingee1!"),
                    EnableSsl = true,
                };

                var from = new MailAddress("compprogtest@gmail.com");
                var to = new MailAddress(request.EmailAddress);

                string emailBody = "<body style=\"margin-left: 24em; margin-right:24em; margin-top: 6em; margin-bottom: 6em; border-style: groove; border-radius:5px\">" +
                                   "<div style=\"background-color: #303d42\">" +
                                   "<h1 style=\"text-align:center; color:white; font-family:Helvetica; margin:0; padding-top: 1em\">CompProgEdu</h1>" +
                                   "<img style=\"display:block; margin-left: auto; margin-right: auto; padding-bottom:1em\" src=\"https://i.imgur.com/mMdNn2h.png\">" +
                                   "</div>" +
                                   "<h2 style=\"text-align:center; padding-top:2em; color:black; font-family:Helvetica\">Welcome to CompProgEdu!</h2>" +
                                   "<div style=\"margin:1em\">" +
                                   "<div style=\"padding:2em\">" +
                                   $"<h4 style=\"color:black; font-family:Helvetica\">Hello, {request.FirstName}</h4>" +
                                   "<h4 style=\"color:black; font-family:Helvetica\">You have been added to a course and an account has been created for you. Log in with your email and temporary password to access your course:</h4>" +
                                   $"<h4 style=\"color:black; font-family:Helvetica; margin-left:3em\"><a rel=\"nofollow\" style=\"text-decoration:none\">Email: {request.EmailAddress}</a></h4>" +
                                   $"<h4 style=\"color: black; font-family:Helvetica; margin-left:3em\">Password: {password}</h4>" +
                                   "<h4 style=\"color:black; font-family:Helvetica\">Thank you,</h4>" +
                                   "<h4 style=\"color:black; font-family:Helvetica\">The CompProgEdu Team</h4>" +
                                   "</div>" +
                                   "</div>";

                var message = new MailMessage(from, to)
                {
                    BodyEncoding = System.Text.Encoding.UTF8,
                    Subject = "CompProgEdu Login Information",
                    Body = emailBody,
                    SubjectEncoding = System.Text.Encoding.UTF8
                };
                message.IsBodyHtml = true;
                smtpClient.Send(message);
            }

            var dto = _mapper.Map<StudentResponseDto>(entity);

            return new ValidateableResponse<StudentResponseDto>(dto);
        }
    }

    public class CreateStudentRequestValidation : AbstractValidator<CreateStudentRequest>
    {
        private readonly DataContext _dataContext;
        public CreateStudentRequestValidation(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
    }
}
