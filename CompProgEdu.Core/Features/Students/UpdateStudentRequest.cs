using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Extensions;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using CompProgEdu.Core.Features.Users;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using CompProgEdu.Core.Security;

namespace CompProgEdu.Core.Features.Students
{
    public class UpdateStudentRequest : StudentUpdateDto, IRequest<ValidateableResponse<StudentResponseDto>>, IValidateable
    {
        public int Id { get; set; }
    }
    public class UpdateStudentRequestHandler : IRequestHandler<UpdateStudentRequest, ValidateableResponse<StudentResponseDto>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public UpdateStudentRequestHandler(
            DataContext dataContext,
            IMapper mapper,
            IMediator mediator)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<ValidateableResponse<StudentResponseDto>> Handle(UpdateStudentRequest request, CancellationToken tkn)
        {
            var entity = _dataContext.Set<Student>().Include(x => x.User).FirstOrDefault(x => x.Id == request.Id);
            var updateUserResponse = await _mediator.Send(new UpdateUserRequest
            {
                Id = entity.UserId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                EmailAddress = entity.User.Email,
                Role = Roles.Student
            }, tkn
            );

            if (updateUserResponse.Errors.Any())
            {
                return new ValidateableResponse<StudentResponseDto>(updateUserResponse.Errors.ToList());
            }

            _mapper.Map(request, entity);

            _dataContext.Update(entity);
            await _dataContext.SaveChangesAsync();

            var dto = _mapper.Map<StudentResponseDto>(entity);

            return new ValidateableResponse<StudentResponseDto>(dto);
        }
    }
    public class UpdateStudentRequestValidation : AbstractValidator<UpdateStudentRequest>
    {
        private readonly DataContext _dataContext;
        public UpdateStudentRequestValidation(DataContext dataContext)
        {
            _dataContext = dataContext;

            RuleFor(x => x.Id)
                .EntityMustExist<UpdateStudentRequest, Student>(_dataContext);
        }
    }
}
