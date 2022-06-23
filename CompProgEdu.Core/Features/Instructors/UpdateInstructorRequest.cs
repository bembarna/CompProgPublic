using AutoMapper;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Extensions;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using CompProgEdu.Core.Features.Users;
using CompProgEdu.Core.Security;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CompProgEdu.Core.Features.Instructors
{
    public class UpdateInstructorRequest : InstructorDetailDto, IRequest<ValidateableResponse<InstructorResponseDto>>, IValidateable
    {
        public int Id { get; set; }

    }

    public class UpdateInstructorRequestHandler : IRequestHandler<UpdateInstructorRequest, ValidateableResponse<InstructorResponseDto>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public UpdateInstructorRequestHandler(
            DataContext dataContext,
            IMapper mapper,
            IMediator mediator)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<ValidateableResponse<InstructorResponseDto>> Handle(UpdateInstructorRequest request, CancellationToken tkn)
        {
            var entity = _dataContext.Set<Instructor>().Include(x => x.User).FirstOrDefault(x => x.Id == request.Id);

            var updateUserResponse = await _mediator.Send(new UpdateUserRequest
            {
                Id = entity.UserId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                EmailAddress = request.EmailAddress,
                Role = Roles.Instructor
            },  tkn
            );

            if (updateUserResponse.Errors.Any())
            {
                return new ValidateableResponse<InstructorResponseDto>(updateUserResponse.Errors.ToList());
            }

            _mapper.Map(request, entity);

            _dataContext.Update(entity);
            await _dataContext.SaveChangesAsync();

            var dto = _mapper.Map<InstructorResponseDto>(entity);

            return new ValidateableResponse<InstructorResponseDto>(dto);
        }
    }

    public class UpdateInstructorRequestValidation : AbstractValidator<UpdateInstructorRequest>
    {
        private readonly DataContext _dataContext;
        public UpdateInstructorRequestValidation(DataContext dataContext)
        {
            _dataContext = dataContext;

            RuleFor(x => x.Id)
                .EntityMustExist<UpdateInstructorRequest, Instructor>(_dataContext);
        }
    }
}
