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
    public class DeleteStudentRequest : IRequest<ValidateableResponse<StudentResponseDto>>, IValidateable
    {
        public int Id { get; set; }
    }

    public class DeleteStudentRequestHandler : IRequestHandler<DeleteStudentRequest, ValidateableResponse<StudentResponseDto>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public DeleteStudentRequestHandler(
            DataContext dataContext,
            IMapper mapper,
            UserManager<User> userManager)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<ValidateableResponse<StudentResponseDto>> Handle(DeleteStudentRequest request, CancellationToken tkn)
        {
            var entity = _dataContext.Set<Student>().Include(x => x.User).FirstOrDefault(x => x.Id == request.Id);

            var dto = _mapper.Map<StudentResponseDto>(entity);

            var linkedStudentsCourses = _dataContext.Set<StudentCourse>().Where(x => x.StudentId == request.Id);

            _dataContext.RemoveRange(linkedStudentsCourses);

            _dataContext.Remove(entity!);

            await _dataContext.SaveChangesAsync(tkn);

            await _userManager.DeleteAsync(entity.User);

            return new ValidateableResponse<StudentResponseDto>(dto);
        }
    }

    public class DeleteStudentRequestValidation : AbstractValidator<DeleteStudentRequest>
    {
        private readonly DataContext _dataContext;

        public DeleteStudentRequestValidation(DataContext dataContext)
        {
            _dataContext = dataContext;

            RuleFor(x => x.Id)
                .EntityMustExist<DeleteStudentRequest, Student>(_dataContext);
        }
    }
}
