using AutoMapper;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Courses;
using CompProgEdu.Core.Features.CurlySets;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Response;
using FluentValidation;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CompProgEdu.Core.Features.Assignments
{
    public class CreateAssignmentRequest : AssignmentDto, IRequest<ValidateableResponse<AssignmentGetDto>>, IValidateable
    {
    }

    public class CreateAssignmentRequestHandler : IRequestHandler<CreateAssignmentRequest, ValidateableResponse<AssignmentGetDto>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public CreateAssignmentRequestHandler(
            DataContext dataContext,
            IMapper mapper,
            IMediator mediator)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<ValidateableResponse<AssignmentGetDto>> Handle(CreateAssignmentRequest request, CancellationToken tkn)
        {
            var entity = _mapper.Map<Assignment>(request);

            await _dataContext.AddAsync(entity, tkn);
            await _dataContext.SaveChangesAsync(tkn);

            var dto = _mapper.Map<AssignmentGetDto>(entity);

            return new ValidateableResponse<AssignmentGetDto>(dto);
        }
    }

    public class CreateAssignmentRequestValidation : AbstractValidator<CreateAssignmentRequest>
    {
        public CreateAssignmentRequestValidation()
        {
            RuleFor(x => x.AssignmentName)
                .NotEmpty();

            RuleFor(x => x.TotalPointValue)
                .NotEmpty();
        }
    }
}

   

