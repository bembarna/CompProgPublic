using AutoMapper;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Courses;
using CompProgEdu.Core.Features.Extensions;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using CompProgEdu.Core.Features.Users;
using CompProgEdu.Core.Security;
using FluentValidation;
using LamarCodeGeneration.Frames;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CompProgEdu.Core.Features.Students
{
    public class AddStudentToCourseRequest : StudentDetailDto, IRequest<ValidateableResponse<StudentResponseDto>>, IValidateable
    {
        public int CourseId { get; set; }
    }

    public class AddStudentToCourseRequestHandler : IRequestHandler<AddStudentToCourseRequest, ValidateableResponse<StudentResponseDto>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IMediator _mediator;

        public AddStudentToCourseRequestHandler(
            DataContext dataContext,
            IMapper mapper,
            UserManager<User> userManager,
            IMediator mediator)
        {
            _dataContext = dataContext;
            _mapper = mapper; ;
            _userManager = userManager;
            _mediator = mediator;
        }


        public async Task<ValidateableResponse<StudentResponseDto>> Handle(AddStudentToCourseRequest request, CancellationToken tkn)
        {
            User searchedUser = await _userManager.FindByEmailAsync(request.EmailAddress);

            StudentResponseDto detailedResponse = null;

            if (searchedUser != null)
            {
                var searchedUsersRole = await _userManager.GetRolesAsync(searchedUser);
                if (searchedUsersRole.Contains(Roles.Student))
                {
                    var studentEntity = _dataContext.Set<Student>().FirstOrDefault(x => x.UserId == searchedUser.Id);

                    var isStudentInCourse = _dataContext.Set<StudentCourse>().FirstOrDefault(x => x.StudentId == studentEntity.Id && x.CourseId == request.CourseId);
                    if(isStudentInCourse != null)
                    {
                        return new ValidateableResponse<StudentResponseDto>("Student is already in Course.", "EmailAddress");
                    }

                    var student = studentEntity;
                    detailedResponse = _mapper.Map<StudentResponseDto>(student);
                }
                else
                {
                    return new ValidateableResponse<StudentResponseDto>("Email entered exists on a non student account", "EmailAddress");
                }
            }
            else
            {
                var studentDto = await _mediator.Send(new CreateStudentRequest
                {
                    EmailAddress = request.EmailAddress,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    StudentSchoolNumber = request.StudentSchoolNumber,
                });

                if (studentDto.Errors.Any())
                {
                    return new ValidateableResponse<StudentResponseDto>(studentDto.Errors.ToList());
                }


                detailedResponse = studentDto.Result;
            }

            var studentCourse = new StudentCourse
            {
                StudentId = detailedResponse.Id,
                CourseId = request.CourseId
            };
            await _dataContext.AddAsync(studentCourse);
            await _dataContext.SaveChangesAsync();

            return new ValidateableResponse<StudentResponseDto>(detailedResponse);
        }
    }

    public class AddStudentToCourseRequestValidation : AbstractValidator<AddStudentToCourseRequest>
    {
        private readonly DataContext _dataContext;
        public AddStudentToCourseRequestValidation(DataContext dataContext)
        {
            _dataContext = dataContext;

            RuleFor(x => x.EmailAddress)
                .NotEmpty();

            RuleFor(x => x.CourseId)
                .EntityMustExist<AddStudentToCourseRequest, Course>(_dataContext);
        }
    }
}
